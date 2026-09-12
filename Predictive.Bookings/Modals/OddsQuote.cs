namespace Predictive.Bookings.Modals
{
    // Decimal odds for one over/under cards line (e.g. line=3.5 → "Over/Under 3.5 cards").
    public class OddsQuote
    {
        public double OverOdds { get; set; }
        public double UnderOdds { get; set; }
    }
}
