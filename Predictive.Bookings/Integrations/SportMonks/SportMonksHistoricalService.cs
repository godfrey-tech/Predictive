using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Modals;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // Builds SeasonMatchRecord history directly from SportMonks instead of CSV. Kept
    // fully separate from EPLHistoricalService/MatchMapper — the existing CSV-based
    // pipeline is untouched. Two uses: (1) evaluating whether SportMonks' available
    // history is deep enough to be a viable CSV alternative (see the 2026-09-13
    // Premier League comparison), and (2) the real, ongoing reason — football-data.
    // co.uk's CSVs only carry a Referee column for English leagues, so this is the
    // only source of historical referee-vs-cards data for Bundesliga/Ligue 1/Serie A/
    // La Liga. Produces the same SeasonMatchRecord type so it plugs into the existing
    // RefereeService/BookingsEngine/BookingsBacktester unchanged.
    public class SportMonksHistoricalService
    {
        private const int RedCardsTypeId = 83;
        private const int YellowCardsTypeId = 84;
        private const int CornersTypeId = 34;
        private const int RefereeTypeId = 6;

        private readonly SportMonksClient _client;

        public SportMonksHistoricalService(SportMonksClient client)
        {
            _client = client;
        }

        public async Task<List<(int id, string name)>> GetSeasons(int leagueId)
        {
            var doc = await _client.GetAsync($"football/leagues/{leagueId}?include=seasons");
            var seasons = new List<(int, string)>();
            if (doc.RootElement.TryGetProperty("data", out var data) &&
                data.TryGetProperty("seasons", out var seasonsEl))
            {
                foreach (var s in seasonsEl.EnumerateArray())
                    seasons.Add((s.GetProperty("id").GetInt32(), s.GetProperty("name").GetString() ?? ""));
            }
            return seasons;
        }

        public async Task<List<SeasonMatchRecord>> LoadSeasonMatches(int seasonId, string leagueIdentifier)
        {
            var records = new List<SeasonMatchRecord>();
            // referees.referee (nested include) returns the referee's name directly on
            // each fixture — avoids a separate per-fixture lookup call.
            string? nextUrl = $"football/fixtures?filters=fixtureSeasons:{seasonId}&include=statistics;referees.referee;participants&per_page=50";

            while (nextUrl != null)
            {
                var doc = await _client.GetAsync(nextUrl);
                foreach (var fixture in doc.RootElement.GetProperty("data").EnumerateArray())
                {
                    var record = MapFixture(fixture, leagueIdentifier);
                    if (record != null) records.Add(record);
                }

                nextUrl = doc.RootElement.TryGetProperty("pagination", out var pg) &&
                          pg.TryGetProperty("next_page", out var np) && np.ValueKind == JsonValueKind.String
                    ? np.GetString()!.Replace("https://api.sportmonks.com/v3/", "")
                    : null;
            }

            return records;
        }

        // Fetching every finished fixture for a league (with stats+referee) is a real
        // API cost (a few hundred paginated calls per league) — cache to disk and
        // reuse across runs instead of refetching every daily run. Deleting the cache
        // file (or letting it exceed maxAge) forces a fresh pull.
        public async Task<List<SeasonMatchRecord>> LoadAllSeasonsCached(
            int leagueId, string leagueIdentifier, string cachePath, TimeSpan maxAge)
        {
            if (File.Exists(cachePath) && DateTime.UtcNow - File.GetLastWriteTimeUtc(cachePath) < maxAge)
                return ReadCache(cachePath);

            var seasons = await GetSeasons(leagueId);
            var all = new List<SeasonMatchRecord>();
            foreach (var (seasonId, _) in seasons)
                all.AddRange(await LoadSeasonMatches(seasonId, leagueIdentifier));

            WriteCache(cachePath, all);
            return all;
        }

        private static void WriteCache(string path, List<SeasonMatchRecord> records)
        {
            using var writer = new StreamWriter(path, append: false);
            writer.WriteLine("Date,HomeTeam,AwayTeam,HomeBookings,AwayBookings,HomeCorners,AwayCorners,Referee");
            foreach (var r in records)
            {
                writer.WriteLine(string.Join(",",
                    r.Date.ToString("yyyy-MM-dd"), Escape(r.HomeTeam), Escape(r.AwayTeam),
                    r.HomeBookings, r.AwayBookings, r.HomeCorners, r.AwayCorners,
                    Escape(r.Referee ?? "")));
            }
        }

        private static List<SeasonMatchRecord> ReadCache(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var p = line.Split(',');
                    return new SeasonMatchRecord
                    {
                        Date = DateTime.Parse(p[0], CultureInfo.InvariantCulture),
                        HomeTeam = p[1],
                        AwayTeam = p[2],
                        HomeBookings = int.Parse(p[3]),
                        AwayBookings = int.Parse(p[4]),
                        HomeCorners = int.Parse(p[5]),
                        AwayCorners = int.Parse(p[6]),
                        Referee = string.IsNullOrEmpty(p[7]) ? null : p[7]
                    };
                })
                .ToList();
        }

        private static string Escape(string value) => value.Contains(',') ? $"\"{value}\"" : value;

        private const int FinishedStateId = 5;

        private static SeasonMatchRecord? MapFixture(JsonElement fixture, string leagueIdentifier)
        {
            // Unplayed/in-progress fixtures have no final statistics — including them
            // would silently record a fake "0 cards" result for every future match.
            if (fixture.GetProperty("state_id").GetInt32() != FinishedStateId) return null;

            if (!fixture.TryGetProperty("participants", out var participants)) return null;

            string? homeTeam = null, awayTeam = null;
            foreach (var p in participants.EnumerateArray())
            {
                string location = p.GetProperty("meta").GetProperty("location").GetString() ?? "";
                string name = SportMonksTeamNameNormalizer.Normalize(p.GetProperty("name").GetString() ?? "");
                if (location == "home") homeTeam = name;
                else if (location == "away") awayTeam = name;
            }
            if (homeTeam == null || awayTeam == null) return null;

            int homeCards = 0, awayCards = 0, homeCorners = 0, awayCorners = 0;
            if (fixture.TryGetProperty("statistics", out var stats))
            {
                foreach (var s in stats.EnumerateArray())
                {
                    int typeId = s.GetProperty("type_id").GetInt32();
                    if (typeId != RedCardsTypeId && typeId != YellowCardsTypeId && typeId != CornersTypeId) continue;

                    int value = s.GetProperty("data").GetProperty("value").GetInt32();
                    bool isHome = s.GetProperty("location").GetString() == "home";

                    if (typeId == CornersTypeId)
                    {
                        if (isHome) homeCorners += value; else awayCorners += value;
                    }
                    else
                    {
                        if (isHome) homeCards += value; else awayCards += value;
                    }
                }
            }

            string? referee = null;
            if (fixture.TryGetProperty("referees", out var referees))
            {
                foreach (var r in referees.EnumerateArray())
                {
                    if (r.GetProperty("type_id").GetInt32() == RefereeTypeId &&
                        r.TryGetProperty("referee", out var refereeObj))
                    {
                        // SportMonks formats this "R. Jones"; strip the period to match
                        // this project's CSV convention ("R Jones"), same as
                        // SportMonksRefereeProvider.
                        referee = refereeObj.GetProperty("common_name").GetString()?.Replace(".", "");
                        break;
                    }
                }
            }

            DateTime date = DateTime.Parse(fixture.GetProperty("starting_at").GetString()!);

            return new SeasonMatchRecord
            {
                LeagueIdentifier = leagueIdentifier,
                Date = date,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeBookings = homeCards,
                AwayBookings = awayCards,
                HomeCorners = homeCorners,
                AwayCorners = awayCorners,
                Referee = referee
            };
        }
    }
}
