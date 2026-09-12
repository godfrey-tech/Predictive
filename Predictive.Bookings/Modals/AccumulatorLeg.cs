namespace Predictive.Bookings.Modals
{
    public class AccumulatorLeg
    {
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public string Market { get; set; } = ""; // e.g. "Total Cards Over 3"
        public double Line { get; set; }         // bookmaker half-line, e.g. 2.5
        public double Probability { get; set; }  // 0-1, calibrated + referee-bias-adjusted
        public double? Odds { get; set; }        // decimal odds, when an IOddsProvider is wired
    }

    public class AccumulatorCandidate
    {
        public List<AccumulatorLeg> Legs { get; set; } = new();
        public double CombinedProbability { get; set; } // 0-1, product of leg probabilities
        public double? CombinedOdds { get; set; }        // product of leg odds, only when every leg has one
        public double? ExpectedValue { get; set; }       // fraction, only when CombinedOdds is known
        public double? KellyFraction { get; set; }        // fraction of bankroll, only when CombinedOdds is known
    }
}
