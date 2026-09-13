using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // Maps a (homeTeam, awayTeam) pair — as spelled in this project's CSVs — to a
    // SportMonks fixture ID, by searching upcoming fixtures in a given league and
    // matching team names against the fixture's "Home vs Away" name string.
    public class SportMonksFixtureResolver
    {
        private readonly SportMonksClient _client;

        // Extend as name mismatches surface between CSV spellings and SportMonks'.
        private static readonly Dictionary<string, string> TeamNameAliases =
            new(StringComparer.OrdinalIgnoreCase)
        {
            { "Man United", "Manchester United" },
            { "Man Utd", "Manchester United" },
            { "Man City", "Manchester City" },
            { "Spurs", "Tottenham Hotspur" },
        };

        public SportMonksFixtureResolver(SportMonksClient client)
        {
            _client = client;
        }

        public async Task<int?> ResolveUpcomingFixtureId(
            string homeTeam, string awayTeam, int leagueId, int daysAhead = 14)
        {
            string home = Alias(homeTeam);
            string away = Alias(awayTeam);

            string from = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string to = DateTime.UtcNow.AddDays(daysAhead).ToString("yyyy-MM-dd");

            var doc = await _client.GetAsync(
                $"football/fixtures/between/{from}/{to}?filters=fixtureLeagues:{leagueId}&per_page=100");

            foreach (var fixture in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                var name = fixture.GetProperty("name").GetString() ?? "";
                if (name.Contains(home, StringComparison.OrdinalIgnoreCase) &&
                    name.Contains(away, StringComparison.OrdinalIgnoreCase))
                {
                    return fixture.GetProperty("id").GetInt32();
                }
            }

            return null;
        }

        private static string Alias(string team)
            => TeamNameAliases.TryGetValue(team, out var alias) ? alias : team;

        // All fixtures in a league on a given date, with team names normalized to
        // this project's CSV convention (via SportMonksTeamNameNormalizer) so the
        // result can be fed straight into BookingsEngine.Analyse.
        public async Task<List<(int fixtureId, string homeTeam, string awayTeam)>> GetFixturesForDate(
            int leagueId, DateTime date)
        {
            string dateStr = date.ToString("yyyy-MM-dd");
            var doc = await _client.GetAsync(
                $"football/fixtures/between/{dateStr}/{dateStr}?filters=fixtureLeagues:{leagueId}&per_page=100");

            var results = new List<(int, string, string)>();
            foreach (var fixture in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                var name = fixture.GetProperty("name").GetString() ?? "";
                var parts = name.Split(" vs ", StringSplitOptions.TrimEntries);
                if (parts.Length != 2) continue;

                int id = fixture.GetProperty("id").GetInt32();
                results.Add((id, SportMonksTeamNameNormalizer.Normalize(parts[0]), SportMonksTeamNameNormalizer.Normalize(parts[1])));
            }
            return results;
        }
    }
}
