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
        private const int RECENT_MONTHS = 20;
        private const int MIN_SAMPLE = 6;

        public BookingsEngine(List<SeasonMatchRecord> data, IRefereeService? refereeService = null, IOddsProvider? oddsProvider = null)
        {
            _data = data;
            _refereeService = refereeService;
            _oddsProvider = oddsProvider;
        }

        // Signal emoji based on percentage
        private string S(double pct)
        {
            if (pct >= 75) return "🟢";
            if (pct >= 65) return "🟡";
            if (pct >= 50) return "🟠";
            return "🔴";
        }

        // Applies a bias multiplier to a hit-rate percentage via an odds-ratio shift
        // (odds = p/(1-p), scaled, converted back) rather than naive multiplication,
        // so a card-happy referee can never push a rate past 100%.
        private static double AdjustRateForBias(double pct, double biasMultiplier)
        {
            if (biasMultiplier == 1.0) return pct;
            double p = Math.Clamp(pct / 100.0, 0.001, 0.999);
            double odds = p / (1 - p) * biasMultiplier;
            return odds / (1 + odds) * 100.0;
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

        public string Analyse(string homeTeam, string awayTeam, string? referee = null)
        {
            var now = DateTime.Now;
            var recentFrom = now.AddMonths(-RECENT_MONTHS);
            var histFrom = now.AddYears(-5);

            var hm = GetMatches(homeTeam, recentFrom, now);
            var am = GetMatches(awayTeam, recentFrom, now);
            var h2h = GetH2H(homeTeam, awayTeam, histFrom, now);

            // Referee bias: known referee + a service to look it up shifts every card
            // number below; unknown referee (the common case pre-SportMonks, since
            // upcoming fixtures rarely have a confirmed referee) leaves them unchanged.
            double refereeBias = (!string.IsNullOrEmpty(referee) && _refereeService != null)
                ? _refereeService.GetRefereeBiasMultiplier(referee)
                : 1.0;

            // ── Card averages ─────────────────────────────
            double homeCardAvg = AvgCards(homeTeam, hm, asHome: true) * refereeBias;
            double awayCardAvg = AvgCards(awayTeam, am, asHome: false) * refereeBias;
            double totalCardAvg = homeCardAvg + awayCardAvg;
            double h2hCardAvg = h2h.Count >= 3
                ? h2h.Average(m => m.HomeBookings + m.AwayBookings) * refereeBias : 0;

            // ── Total match card rates ────────────────────
            double ft_o3 = AdjustRateForBias(TotalCardRate(hm, am, t: 2), refereeBias);  // Over 3 = 3+ cards
            double ft_o4 = AdjustRateForBias(TotalCardRate(hm, am, t: 3), refereeBias);  // Over 4 = 4+ cards
            double ft_o5 = AdjustRateForBias(TotalCardRate(hm, am, t: 4), refereeBias);  // Over 5 = 5+ cards
            double ft_o6 = AdjustRateForBias(TotalCardRate(hm, am, t: 5), refereeBias);  // Over 6 = 6+ cards

            // ── Home card rates ───────────────────────────
            double h_o1 = AdjustRateForBias(CardRate(homeTeam, hm, asHome: true, t: 0), refereeBias);
            double h_o2 = AdjustRateForBias(CardRate(homeTeam, hm, asHome: true, t: 1), refereeBias);
            double h_o3 = AdjustRateForBias(CardRate(homeTeam, hm, asHome: true, t: 2), refereeBias);

            // ── Away card rates ───────────────────────────
            double a_o1 = AdjustRateForBias(CardRate(awayTeam, am, asHome: false, t: 0), refereeBias);
            double a_o2 = AdjustRateForBias(CardRate(awayTeam, am, asHome: false, t: 1), refereeBias);
            double a_o3 = AdjustRateForBias(CardRate(awayTeam, am, asHome: false, t: 2), refereeBias);

            // ── Corner averages ───────────────────────────
            double homeCornerAvg = AvgCorners(homeTeam, hm, asHome: true);
            double awayCornerAvg = AvgCorners(awayTeam, am, asHome: false);
            double totalCornerAvg = homeCornerAvg + awayCornerAvg;
            double h2hCornerAvg = h2h.Count >= 3
                ? h2h.Average(m => m.HomeCorners + m.AwayCorners) : 0;

            // ── Corner hit rates ──────────────────────────
            double c_o7 = CornerRate(hm, am, t: 6);
            double c_o8 = CornerRate(hm, am, t: 7);
            double c_o9 = CornerRate(hm, am, t: 8);
            double c_o10 = CornerRate(hm, am, t: 9);
            double c_o11 = CornerRate(hm, am, t: 10);

            // ── Build output ──────────────────────────────
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("🟢 STRONG(>=75%)  🟡 MODERATE(65-74%)  🟠 BORDERLINE(50-64%)  🔴 WEAK(<50%)");
            if (!string.IsNullOrEmpty(referee) && refereeBias != 1.0)
                sb.AppendLine($"Referee: {referee}  (bias x{refereeBias:F2})");
            sb.AppendLine();

            // Cards section
            sb.AppendLine("BOOKINGS & CARDS");
            sb.Append($"Home Avg: {homeCardAvg:F2}/game  |  Away Avg: {awayCardAvg:F2}/game  |  Combined: {totalCardAvg:F2}/game");
            if (h2h.Count >= 3) sb.Append($"  |  H2H Avg: {h2hCardAvg:F2}/game ({h2h.Count} matches)");
            sb.AppendLine();
            sb.AppendLine($"Total Match Cards:");
            if (_oddsProvider == null)
            {
                sb.AppendLine($"  Over 3: {S(ft_o3)}{ft_o3:F1}%   Over 4: {S(ft_o4)}{ft_o4:F1}%   Over 5: {S(ft_o5)}{ft_o5:F1}%   Over 6: {S(ft_o6)}{ft_o6:F1}%");
            }
            else
            {
                sb.AppendLine($"  {FormatCardLine("Over 3", ft_o3, 2, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 4", ft_o4, 3, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 5", ft_o5, 4, homeTeam, awayTeam)}");
                sb.AppendLine($"  {FormatCardLine("Over 6", ft_o6, 5, homeTeam, awayTeam)}");
            }
            sb.AppendLine($"{homeTeam} Cards:");
            sb.AppendLine($"  Over 1: {S(h_o1)}{h_o1:F1}%   Over 2: {S(h_o2)}{h_o2:F1}%   Over 3: {S(h_o3)}{h_o3:F1}%");
            sb.AppendLine($"{awayTeam} Cards:");
            sb.AppendLine($"  Over 1: {S(a_o1)}{a_o1:F1}%   Over 2: {S(a_o2)}{a_o2:F1}%   Over 3: {S(a_o3)}{a_o3:F1}%");
            sb.AppendLine();

            // Corners section
            sb.AppendLine("CORNERS");
            sb.Append($"Home Avg: {homeCornerAvg:F2}/game  |  Away Avg: {awayCornerAvg:F2}/game  |  Combined: {totalCornerAvg:F2}/game");
            if (h2h.Count >= 3) sb.Append($"  |  H2H Avg: {h2hCornerAvg:F2}/game ({h2h.Count} matches)");
            sb.AppendLine();
            sb.AppendLine($"  Over 7:  {S(c_o7)}{c_o7:F1}%   Over 8:  {S(c_o8)}{c_o8:F1}%   Over 9:  {S(c_o9)}{c_o9:F1}%   Over 10: {S(c_o10)}{c_o10:F1}%   Over 11: {S(c_o11)}{c_o11:F1}%");
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
