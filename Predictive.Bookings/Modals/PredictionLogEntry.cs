namespace Predictive.Bookings.Modals
{
    // One row of the running prediction/accuracy log (Data/Results/PredictionLog.csv).
    // Written when a prediction is made; Checked/actual fields filled in later once
    // the fixture has been played.
    public class PredictionLogEntry
    {
        public DateTime Date { get; set; }
        public string League { get; set; } = "";
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public int FixtureId { get; set; }
        public string Referee { get; set; } = "";

        public double FtOver3Pred { get; set; }
        public double FtOver4Pred { get; set; }
        public double FtOver5Pred { get; set; }
        public double FtOver6Pred { get; set; }

        // Null until checked against the actual result.
        public int? ActualTotalCards { get; set; }
    }
}
