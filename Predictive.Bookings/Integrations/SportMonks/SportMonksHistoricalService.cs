using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Modals;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // Prototype: builds SeasonMatchRecord history directly from SportMonks instead of
    // CSV. Kept fully separate from EPLHistoricalService/MatchMapper — the existing
    // CSV-based pipeline is untouched, this is purely for evaluating whether
    // SportMonks' available history (currently 3 seasons on the Starter plan for
    // Premier League) is deep enough to be a viable alternative. Produces the same
    // SeasonMatchRecord type so it can be fed into the existing BookingsEngine /
    // BookingsBacktester unchanged for an apples-to-apples comparison.
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
            string? nextUrl = $"football/fixtures?filters=fixtureSeasons:{seasonId}&include=statistics;referees;participants&per_page=50";

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
                    if (r.GetProperty("type_id").GetInt32() == RefereeTypeId)
                    {
                        // Referee name isn't included here (only referee_id) — left null
                        // for this prototype; a full version would resolve it the same
                        // way SportMonksRefereeProvider does.
                        referee = null;
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
