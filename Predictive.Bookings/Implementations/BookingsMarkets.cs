namespace Predictive.Bookings.Implementations
{
    // Market bucket keys shared between BookingsEngine (applies calibration) and
    // BookingsBacktester (fits calibration) so the two never drift out of sync.
    public static class BookingsMarkets
    {
        public const string Over3 = "TotalCardsOver3";
        public const string Over4 = "TotalCardsOver4";
        public const string Over5 = "TotalCardsOver5";
        public const string Over6 = "TotalCardsOver6";
        public const string HomeOver1 = "HomeCardsOver1";
        public const string HomeOver2 = "HomeCardsOver2";
        public const string HomeOver3 = "HomeCardsOver3";
        public const string AwayOver1 = "AwayCardsOver1";
        public const string AwayOver2 = "AwayCardsOver2";
        public const string AwayOver3 = "AwayCardsOver3";

        public static readonly string[] All =
        {
            Over3, Over4, Over5, Over6,
            HomeOver1, HomeOver2, HomeOver3,
            AwayOver1, AwayOver2, AwayOver3
        };
    }
}
