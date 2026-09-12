namespace Predictive.Bookings.Modals
{
    // Raw numeric output of BookingsEngine.Predict — kept separate from the
    // formatted Analyse() string so a backtester can score predictions directly
    // instead of parsing text.
    public class BookingsPrediction
    {
        public double HomeCardAvg { get; set; }
        public double AwayCardAvg { get; set; }
        public double TotalCardAvg { get; set; }
        public double H2HCardAvg { get; set; }
        public int H2HCount { get; set; }

        public double FtOver3 { get; set; }
        public double FtOver4 { get; set; }
        public double FtOver5 { get; set; }
        public double FtOver6 { get; set; }

        public double HomeOver1 { get; set; }
        public double HomeOver2 { get; set; }
        public double HomeOver3 { get; set; }

        public double AwayOver1 { get; set; }
        public double AwayOver2 { get; set; }
        public double AwayOver3 { get; set; }

        public double HomeCornerAvg { get; set; }
        public double AwayCornerAvg { get; set; }
        public double TotalCornerAvg { get; set; }
        public double H2HCornerAvg { get; set; }

        public double CornerOver7 { get; set; }
        public double CornerOver8 { get; set; }
        public double CornerOver9 { get; set; }
        public double CornerOver10 { get; set; }
        public double CornerOver11 { get; set; }

        public double RefereeBias { get; set; } = 1.0;
    }
}
