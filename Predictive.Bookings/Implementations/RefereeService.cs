using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Modals;
using Predictive.Bookings.Interfaces;

namespace Predictive.Bookings.Implementations
{
    public class RefereeService : IRefereeService
    {
        private readonly List<SeasonMatchRecord> _data;
        private readonly double _leagueAverageBookings;
        private const int MIN_SAMPLE = 5;
        private const double CARD_HAPPY_THRESHOLD = 1.15; // 15% above league average

        public RefereeService(List<SeasonMatchRecord> data)
        {
            _data = data;
            _leagueAverageBookings = _data.Count > 0
                ? _data.Average(m => m.HomeBookings + m.AwayBookings)
                : 3.8; // matches BookingsEngine's own fallback (1.90 * 2)
        }

        public double GetAverageBookingsPerMatch(string referee)
        {
            var matches = MatchesFor(referee);
            return matches.Count >= MIN_SAMPLE
                ? matches.Average(m => m.HomeBookings + m.AwayBookings)
                : _leagueAverageBookings;
        }

        public bool IsRefereeCardHappy(string referee)
        {
            var matches = MatchesFor(referee);
            if (matches.Count < MIN_SAMPLE) return false;
            return GetAverageBookingsPerMatch(referee) >= _leagueAverageBookings * CARD_HAPPY_THRESHOLD;
        }

        public double GetRefereeBiasMultiplier(string referee)
        {
            var matches = MatchesFor(referee);
            if (matches.Count < MIN_SAMPLE || _leagueAverageBookings <= 0) return 1.0;
            return GetAverageBookingsPerMatch(referee) / _leagueAverageBookings;
        }

        private List<SeasonMatchRecord> MatchesFor(string referee)
            => _data.Where(m => string.Equals(m.Referee, referee, System.StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
