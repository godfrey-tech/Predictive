using System.Threading.Tasks;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // Resolves the referee assigned to an upcoming (unplayed) fixture — the one thing
    // historical CSVs structurally can't provide, since they only fill in Referee
    // after a match is played. Works on the base plan; no odds add-on required.
    public class SportMonksRefereeProvider
    {
        private const int RefereeTypeId = 6; // confirmed via /v3/core/types

        private readonly SportMonksClient _client;

        public SportMonksRefereeProvider(SportMonksClient client)
        {
            _client = client;
        }

        public async Task<string?> GetRefereeNameForFixture(int fixtureId)
        {
            var doc = await _client.GetAsync($"football/fixtures/{fixtureId}?include=referees");
            if (!doc.RootElement.TryGetProperty("data", out var data)) return null;
            if (!data.TryGetProperty("referees", out var referees)) return null;

            foreach (var r in referees.EnumerateArray())
            {
                if (r.GetProperty("type_id").GetInt32() == RefereeTypeId)
                {
                    int refereeId = r.GetProperty("referee_id").GetInt32();
                    return await GetRefereeName(refereeId);
                }
            }

            return null;
        }

        private async Task<string?> GetRefereeName(int refereeId)
        {
            var doc = await _client.GetAsync($"football/referees/{refereeId}");
            if (!doc.RootElement.TryGetProperty("data", out var data)) return null;

            // SportMonks formats this "R. Jones"; this project's CSVs use "R Jones" —
            // strip the period so it matches RefereeService's historical lookups.
            var commonName = data.GetProperty("common_name").GetString();
            return commonName?.Replace(".", "");
        }
    }
}
