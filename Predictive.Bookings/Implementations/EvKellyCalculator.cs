using System;

namespace Predictive.Bookings.Implementations
{
    // Pure functions — no engine/data dependency, so easy to unit-test directly.
    public static class EvKellyCalculator
    {
        // Expected value as a fraction of stake, e.g. 0.20 = +20% EV.
        public static double CalculateEV(double probability, double decimalOdds)
            => probability * decimalOdds - 1.0;

        // Fraction of bankroll to stake, using fractional Kelly (default quarter-Kelly —
        // full Kelly is too volatile for real staking). 0 when there's no edge.
        public static double CalculateKellyFraction(double probability, double decimalOdds, double fractionalKelly = 0.25)
        {
            double b = decimalOdds - 1.0; // net odds
            if (b <= 0) return 0;
            double kelly = (probability * b - (1 - probability)) / b;
            return Math.Max(0, kelly) * fractionalKelly;
        }

        // Convenience wrapper: fraction of bankroll converted to a stake amount.
        public static double CalculateKellyStake(double probability, double decimalOdds, double bankroll, double fractionalKelly = 0.25)
            => CalculateKellyFraction(probability, decimalOdds, fractionalKelly) * bankroll;
    }
}
