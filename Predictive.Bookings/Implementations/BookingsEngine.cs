using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Modals;
using Predictive.Bookings.Interfaces;
using Predictive.Bookings.Modals;

// ============================================================
//  BookingsEngine.cs — Predictive.Bookings/Implementations/
//  Advanced cards/bookings engine — starting point copied from
//  ConsoleApp1's BookingsEngine, enhanced here with referee bias,
//  real odds/EV, calibration and accumulators (see docs/plan).
//  Cards start from Over 3 | Corners start from Over 7/8
//  Signal: 🟢 >=75%  🟡 65-74%  🟠 50-64%  🔴 <50%
// ============================================================

namespace Predictive.Bookings.Implementations
{
    public class BookingsEngine
    {
        private readonly List<SeasonMatchRecord> _data;
        private readonly IRefereeService? _refereeService;
        private readonly IOddsProvider? _oddsProvider;
        private readonly Dictionary<string, ConfidenceCalibrator>? _calibrators;
        private const int RECENT_MONTHS = 20;
        private const int MIN_SAMPLE = 6;

        public BookingsEngine(
            List<SeasonMatchRecord> data,
            IRefereeService? refereeService = null,
            IOddsProvider? oddsProvider = null,
            Dictionary<string, ConfidenceCalibrator>? calibrators = null)
        {
            _data = data;
            _refereeService = refereeService;
            _oddsProvider = oddsProvider;
            _calibrators = calibrators;
        }

        // Signal emoji based on percentage
        private string S(double pct)
        {
            if (pct >= 75) return "🟢";
            if (pct >= 65) return "🟡";
            if (pct >= 50) return "🟠";
            return "🔴";
        }

        // Applies a multiplier to a hit-rate percentage via an odds-ratio shift
        // (odds = p/(1-p), scaled, converted back) rather than naive multiplication,
        // so a factor like referee bias can never push a rate past 100%.
        private static double AdjustRateForBias(double pct, double biasMultiplier)
        {
            if (biasMultiplier == 1.0) return pct;
            double p = Math.Clamp(pct / 100.0, 0.001, 0.999);
            double odds = p / (1 - p) * biasMultiplier;
            return odds / (1 + odds) * 100.0;
        }

        private double Calibrate(string market, double rawPct)
            => _calibrators != null && _calibrators.TryGetValue(market, out var c)
                ? c.Calibrate(rawPct)
                : rawPct;

        // Raw numeric prediction: calibration is applied first (corrects the engine's
        // own systematic bias), then referee bias (a real-world per-match effect) on
        // top. Kept separate from Analyse() so BookingsBacktester can score
        // predictions directly instead of parsing formatted text.
        public BookingsPrediction Predict(string homeTeam, string awayTeam, string? referee = null, DateTime? asOf = null)
        {
            var now = asOf ?? DateTime.Now;
            var recentFrom = now.AddMonths(-RECENT_MONTHS);
            var histFrom = now.AddYears(-5);

            var hm = GetMatches(homeTeam, recentFrom, now);
            var am = GetMatches(awayTeam, recentFrom, now);
            var h2h = GetH2H(homeTeam, awayTeam, histFrom, now);

            double refereeBias = (!string.IsNullOrEmpty(referee) && _refereeService != null)
                ? _refereeService.GetRefereeBiasMultiplier(referee)
                : 1.0;

            double homeCardAvg = AvgCards(homeTeam, hm, asHome: true) * refereeBias;
            double awayCardAvg = AvgCards(awayTeam, am, asHome: false) * refereeBias;
            double h2hCardAvg = h2h.Count >= 3
                ? h2h.Average(m => m.HomeBookings + m.AwayBookings) * refereeBias : 0;

            double ft_o3 = AdjustRateForBias(Calibrate(BookingsMarkets.Over3, TotalCardRate(hm, am, t: 2)), refereeBias);
            double ft_o4 = AdjustRateForBias(Calibrate(BookingsMarkets.Over4, TotalCardRate(hm, am, t: 3)), refereeBias);
            double ft_o5 = AdjustRateForBias(Calibrate(BookingsMarkets.Over5, TotalCardRate(hm, am, t: 4)), refereeBias);
            double ft_o6 = AdjustRateForBias(Calibrate(BookingsMarkets.Over6, TotalCardRate(hm, am, t: 5)), refereeBias);

            double h_o1 = AdjustRateForBias(Calibrate(BookingsMarkets.HomeOver1, CardRate(homeTeam, hm, asHome: true, t: 0)), refereeBias);
            double h_o2 = AdjustRateForBias(Calibrate(BookingsMarkets.HomeOver2, CardRate(homeTeam, hm, asHome: true, t: 1)), refereeBias);
            double h_o3 = AdjustRateForBias(Calibrate(BookingsMarkets.HomeOver3, CardRate(homeTeam, hm, asHome: true, t: 2)), refereeBias);

            double a_o1 = AdjustRateForBias(Calibrate(BookingsMarkets.AwayOver1, CardRate(awayTeam, am, asHome: false, t: 0)), refereeBias);
            double a_o2 = AdjustRateForBias(Calibrate(BookingsMarkets.AwayOver2, CardRate(awayTeam, am, asHome: false, t: 1)), refereeBias);
            double a_o3 = AdjustRateForBias(Calibrate(BookingsMarkets.AwayOver3, CardRate(awayTeam, am, asHome: false, t: 2)), refereeBias);

            double homeCornerAvg = AvgCorners(homeTeam, hm, asHome: true);
            double awayCornerAvg = AvgCorners(awayTeam, am, asHome: false);
            double h2hCornerAvg = h2h.Count >= 3
                ? h2h.Average(m => m.HomeCorners + m.AwayCorners) : 0;

            return new BookingsPrediction
            {
                HomeCardAvg = homeCardAvg,
                AwayCardAvg = awayCardAvg,
                TotalCardAvg = homeCardAvg + awayCardAvg,
                H2HCardAvg = h2hCardAvg,
                H2HCount = h2h.Count,

                FtOver3 = ft_o3,
                FtOver4 = ft_o4,
                FtOver5 = ft_o5,
                FtOver6 = ft_o6,

                HomeOver1 = h_o1,
                HomeOver2 = h_o2,
                HomeOver3 = h_o3,

                AwayOver1 = a_o1,
                AwayOver2 = a_o2,
                AwayOver3 = a_o3,

                HomeCornerAvg = homeCornerAvg,
                AwayCornerAvg = awayCornerAvg,
                TotalCornerAvg = homeCornerAvg + awayCornerAvg,
                H2HCornerAvg = h2hCornerAvg,

                CornerOver7 = CornerRate(hm, am, t: 6),
                CornerOver8 = CornerRate(hm, am, t: 7),
                CornerOver9 = CornerRate(hm, am, t: 8),
                CornerOver10 = CornerRate(hm, am, t: 9),
                CornerOver11 = CornerRate(hm, am, t: 10),

                RefereeBias = refereeBias
            };
        }

        // "Over 3" (t=2) means 3+ cards, i.e. the standard bookmaker "Over 2.5" line.
        // When an odds quote is available for that line, appends EV% and a suggested
        // Kelly stake (% of bankroll); otherwise returns the plain hit-rate text
        // unchanged, so behaviour is identical to before odds were wired in.
        private string FormatCardLine(string label, double pct, int t, string homeTeam, string awayTeam)
        {
            string baseText = $"{label}: {S(pct)}{pct:F1}%";
            var quote = _oddsProvider?.GetCardsOdds(homeTeam, awayTeam, t + 0.5);
            if (quote == null) return baseText;

            double p = pct / 100.0;
            double ev = EvKellyCalculator.CalculateEV(p, quote.OverOdds) * 100;
            double kellyPct = EvKellyCalculator.CalculateKellyFraction(p, quote.OverOdds) * 100;
            string evText = ev >= 0 ? $"+{ev:F1}%" : $"{ev:F1}%";
            return $"{baseText} @{quote.OverOdds:F2} (EV {evText}, Kelly {kellyPct:F1}% bankroll)";
        }

        public string Analyse(string homeTeam, string awayTeam, string? referee = null, DateTime? asOf = null)
        {
            var pred = Predict(homeTeam, awayTeam, referee, asOf);

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("🟢 STRONG(>=75%)  🟡 MODERATE(65-74%)  🟠 BORDERLINE(50-64%)  🔴 WEAK(<50%)");
            if (!string.IsNullOrEmpty(referee) && pred.RefereeBias != 1.0)
                sb.AppendLine($"Referee: {referee}  (bias x{pred.RefereeBias:F2})");
            sb.AppendLine();

            // Cards section
            sb.AppendLine("BOOKINGS & CARDS");
            sb.Append($"Home Avg: {pred.HomeCardAvg:F2}/game  |  Away Avg: {pred.AwayCardAvg:F2}/game  |  Combined: {pred.TotalCardAvg:F2}/game");
            if (pred.H2HCount >= 3) sb.Append($"  |  H2H Avg: {pred.H2HCardAvg:F2}/game ({pred.H2HCount} matches)");
            sb.AppendLine();
            sb.AppendLine($"Total Match Cards:");
            if (_oddsProvider == null)
            {
                sb.AppendLine($"  Over 3: {S(pred.FtOver3)}{pred.FtOver3:F1}%   Over 4: {S(pred.FtOver4)}{pred.FtOver4:F1}%   Over 5: {S(pred.FtOver5)}{pred.FtOver5:F1}%   Over 6: {S(pred.FtOver6)}{pred.FtOver6:F1}%");
            }
            else
            {
                sb.AppendLine($"  {FormatCardLine("Over 3", pred.FtOver3, 2, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 4", pred.FtOver4, 3, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 5", pred.FtOver5, 4, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 6", pred.FtOver6, 5, homeTeam, awayTeam)}");
            }
            sb.AppendLine($"{homeTeam} Cards:");
            sb.AppendLine($"  Over 1: {S(pred.HomeOver1)}{pred.HomeOver1:F1}%   Over 2: {S(pred.HomeOver2)}{pred.HomeOver2:F1}%   Over 3: {S(pred.HomeOver3)}{pred.HomeOver3:F1}%");
            sb.AppendLine($"{awayTeam} Cards:");
            sb.AppendLine($"  Over 1: {S(pred.AwayOver1)}{pred.AwayOver1:F1}%   Over 2: {S(pred.AwayOver2)}{pred.AwayOver2:F1}%   Over 3: {S(pred.AwayOver3)}{pred.AwayOver3:F1}%");
            sb.AppendLine();

            // Corners section
            sb.AppendLine("CORNERS");
            sb.Append($"Home Avg: {pred.HomeCornerAvg:F2}/game  |  Away Avg: {pred.AwayCornerAvg:F2}/game  |  Combined: {pred.TotalCornerAvg:F2}/game");
            if (pred.H2HCount >= 3) sb.Append($"  |  H2H Avg: {pred.H2HCornerAvg:F2}/game ({pred.H2HCount} matches)");
            sb.AppendLine();
            sb.AppendLine($"  Over 7:  {S(pred.CornerOver7)}{pred.CornerOver7:F1}%   Over 8:  {S(pred.CornerOver8)}{pred.CornerOver8:F1}%   Over 9:  {S(pred.CornerOver9)}{pred.CornerOver9:F1}%   Over 10: {S(pred.CornerOver10)}{pred.CornerOver10:F1}%   Over 11: {S(pred.CornerOver11)}{pred.CornerOver11:F1}%");
            sb.AppendLine();

            return sb.ToString();
        }

        // ════════════════════════════════════════════════════
        //  CALCULATIONS — CARDS
        // ════════════════════════════════════════════════════

        private double AvgCards(string team, List<SeasonMatchRecord> matches, bool asHome)
        {
            if (matches.Count < MIN_SAMPLE) return 1.90;
            return matches.Average(m => asHome
                ? (m.HomeTeam == team ? m.HomeBookings : m.AwayBookings)
                : (m.AwayTeam == team ? m.AwayBookings : m.HomeBookings));
        }

        private double CardRate(string team, List<SeasonMatchRecord> matches, bool asHome, int t)
        {
            if (matches.Count < MIN_SAMPLE)
                return t == 0 ? 85.0 : t == 1 ? 60.0 : 35.0;
            int count = matches.Count(m =>
            {
                int cards = asHome
                    ? (m.HomeTeam == team ? m.HomeBookings : m.AwayBookings)
                    : (m.AwayTeam == team ? m.AwayBookings : m.HomeBookings);
                return cards > t;
            });
            return count / (double)matches.Count * 100;
        }

        private double TotalCardRate(List<SeasonMatchRecord> hm, List<SeasonMatchRecord> am, int t)
        {
            var all = hm.Concat(am)
                        .GroupBy(m => $"{m.HomeTeam}_{m.AwayTeam}_{m.Date:yyyyMMdd}")
                        .Select(g => g.First()).ToList();
            if (all.Count < MIN_SAMPLE)
                return t == 2 ? 76.0 : t == 3 ? 56.0 : t == 4 ? 37.0 : 22.0;
            return all.Count(m => m.HomeBookings + m.AwayBookings > t)
                   / (double)all.Count * 100;
        }

        // ════════════════════════════════════════════════════
        //  CALCULATIONS — CORNERS
        // ════════════════════════════════════════════════════

        private double AvgCorners(string team, List<SeasonMatchRecord> matches, bool asHome)
        {
            if (matches.Count < MIN_SAMPLE) return 4.5;
            return matches.Average(m => asHome
                ? (m.HomeTeam == team ? m.HomeCorners : m.AwayCorners)
                : (m.AwayTeam == team ? m.AwayCorners : m.HomeCorners));
        }

        private double CornerRate(List<SeasonMatchRecord> hm, List<SeasonMatchRecord> am, int t)
        {
            var all = hm.Concat(am)
                        .GroupBy(m => $"{m.HomeTeam}_{m.AwayTeam}_{m.Date:yyyyMMdd}")
                        .Select(g => g.First()).ToList();
            if (all.Count < MIN_SAMPLE)
                return t == 6 ? 80.0 : t == 7 ? 69.0 : t == 8 ? 57.0 : t == 9 ? 47.0 : 38.0;
            return all.Count(m => m.HomeCorners + m.AwayCorners > t)
                   / (double)all.Count * 100;
        }

        // ════════════════════════════════════════════════════
        //  DATA HELPERS
        // ════════════════════════════════════════════════════

        private List<SeasonMatchRecord> GetMatches(string team, DateTime from, DateTime to)
            => _data.Where(m => (m.HomeTeam == team || m.AwayTeam == team)
                                && m.Date >= from && m.Date <= to).ToList();

        private List<SeasonMatchRecord> GetH2H(string a, string b, DateTime from, DateTime to)
            => _data.Where(m => ((m.HomeTeam == a && m.AwayTeam == b) ||
                                 (m.HomeTeam == b && m.AwayTeam == a))
                                && m.Date >= from && m.Date <= to).ToList();
    }
}
