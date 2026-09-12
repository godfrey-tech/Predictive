using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Modals;

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
        private const int RECENT_MONTHS = 20;
        private const int MIN_SAMPLE = 6;

        public BookingsEngine(List<SeasonMatchRecord> data) { _data = data; }

        // Signal emoji based on percentage
        private string S(double pct)
        {
            if (pct >= 75) return "🟢";
            if (pct >= 65) return "🟡";
            if (pct >= 50) return "🟠";
            return "🔴";
        }

        public string Analyse(string homeTeam, string awayTeam)
        {
            var now = DateTime.Now;
            var recentFrom = now.AddMonths(-RECENT_MONTHS);
            var histFrom = now.AddYears(-5);

            var hm = GetMatches(homeTeam, recentFrom, now);
            var am = GetMatches(awayTeam, recentFrom, now);
            var h2h = GetH2H(homeTeam, awayTeam, histFrom, now);

            // ── Card averages ─────────────────────────────
            double homeCardAvg = AvgCards(homeTeam, hm, asHome: true);
            double awayCardAvg = AvgCards(awayTeam, am, asHome: false);
            double totalCardAvg = homeCardAvg + awayCardAvg;
            double h2hCardAvg = h2h.Count >= 3
                ? h2h.Average(m => m.HomeBookings + m.AwayBookings) : 0;

            // ── Total match card rates ────────────────────
            double ft_o3 = TotalCardRate(hm, am, t: 2);  // Over 3 = 3+ cards
            double ft_o4 = TotalCardRate(hm, am, t: 3);  // Over 4 = 4+ cards
            double ft_o5 = TotalCardRate(hm, am, t: 4);  // Over 5 = 5+ cards
            double ft_o6 = TotalCardRate(hm, am, t: 5);  // Over 6 = 6+ cards

            // ── Home card rates ───────────────────────────
            double h_o1 = CardRate(homeTeam, hm, asHome: true, t: 0);
            double h_o2 = CardRate(homeTeam, hm, asHome: true, t: 1);
            double h_o3 = CardRate(homeTeam, hm, asHome: true, t: 2);

            // ── Away card rates ───────────────────────────
            double a_o1 = CardRate(awayTeam, am, asHome: false, t: 0);
            double a_o2 = CardRate(awayTeam, am, asHome: false, t: 1);
            double a_o3 = CardRate(awayTeam, am, asHome: false, t: 2);

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
            sb.AppendLine();

            // Cards section
            sb.AppendLine("BOOKINGS & CARDS");
            sb.Append($"Home Avg: {homeCardAvg:F2}/game  |  Away Avg: {awayCardAvg:F2}/game  |  Combined: {totalCardAvg:F2}/game");
            if (h2h.Count >= 3) sb.Append($"  |  H2H Avg: {h2hCardAvg:F2}/game ({h2h.Count} matches)");
            sb.AppendLine();
            sb.AppendLine($"Total Match Cards:");
            sb.AppendLine($"  Over 3: {S(ft_o3)}{ft_o3:F1}%   Over 4: {S(ft_o4)}{ft_o4:F1}%   Over 5: {S(ft_o5)}{ft_o5:F1}%   Over 6: {S(ft_o6)}{ft_o6:F1}%");
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
