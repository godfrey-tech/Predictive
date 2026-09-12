namespace Predictive.Bookings.Interfaces
{
    public interface IRefereeService
    {
        // Average total cards (home + away) per match this referee has officiated.
        double GetAverageBookingsPerMatch(string referee);

        // True when this referee's average is meaningfully above the league average.
        bool IsRefereeCardHappy(string referee);

        // Referee's average ÷ league average — the multiplier BookingsEngine applies
        // to its historical card rates when this referee is known for an upcoming match.
        // 1.0 (neutral) when the referee is unknown or has too small a sample.
        double GetRefereeBiasMultiplier(string referee);
    }
}
