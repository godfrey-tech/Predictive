using Predictive.Bookings.Modals;

namespace Predictive.Bookings.Interfaces
{
    public interface IOddsProvider
    {
        // line follows standard bookmaker half-line convention (e.g. 3.5 = "Over/Under
        // 3.5 cards", i.e. 4+ counts as over). Returns null when no quote is available
        // for this fixture/line — historical CSVs don't carry cards-market odds at all,
        // so this is expected until a live odds source (SportMonks) is wired in.
        OddsQuote? GetCardsOdds(string homeTeam, string awayTeam, double line);
    }
}
