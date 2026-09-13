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
    }
}
