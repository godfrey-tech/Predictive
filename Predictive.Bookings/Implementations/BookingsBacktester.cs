using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp1.Modals;

namespace Predictive.Bookings.Implementations
{
    public class BookingsBacktestSummary
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int MatchesAnalysed { get; set; }
        public Dictionary<string, ConfidenceCalibrator> Calibrators { get; set; } = new();
        public string Report { get; set; } = "";
    }

    // Walk-forward backtest: for each historical match, retrains BookingsEngine on
    // only the data before that match's date (same no-lookahead pattern as
    // HTFTBacktester), records (rawPredictedPct, actualHit) per market bucket, then
    // fits a ConfidenceCalibrator per bucket. This is how the engine's raw hit-rates
    // get corrected against what actually happened historically.
    public class BookingsBacktester
    {
        private readonly List<SeasonMatchRecord> _allData;
        private const int MIN_TRAINING_MATCHES = 100;

        public BookingsBacktester(List<SeasonMatchRecord> allData)
        {
            _allData = allData;
        }

        public BookingsBacktestSummary Run(DateTime from, DateTime to)
        {
            var testMatches = _allData
                .Where(m => m.Date >= from && m.Date <= to)
                .OrderBy(m => m.Date)
                .ToList();

            var samples = BookingsMarkets.All.ToDictionary(m => m, _ => new List<(double rawPct, bool won)>());

            int analysed = 0;
            foreach (var match in testMatches)
            {
                // No lookahead: only train on matches strictly before this one.
                var trainingData = _allData.Where(m => m.Date < match.Date).ToList();
                if (trainingData.Count < MIN_TRAINING_MATCHES) continue;

                var engine = new BookingsEngine(trainingData);
                var pred = engine.Predict(match.HomeTeam, match.AwayTeam, referee: null, asOf: match.Date);
                analysed++;

                int totalCards = match.HomeBookings + match.AwayBookings;
                samples[BookingsMarkets.Over3].Add((pred.FtOver3, totalCards > 2));
                samples[BookingsMarkets.Over4].Add((pred.FtOver4, totalCards > 3));
                samples[BookingsMarkets.Over5].Add((pred.FtOver5, totalCards > 4));
                samples[BookingsMarkets.Over6].Add((pred.FtOver6, totalCards > 5));
                samples[BookingsMarkets.HomeOver1].Add((pred.HomeOver1, match.HomeBookings > 0));
                samples[BookingsMarkets.HomeOver2].Add((pred.HomeOver2, match.HomeBookings > 1));
                samples[BookingsMarkets.HomeOver3].Add((pred.HomeOver3, match.HomeBookings > 2));
                samples[BookingsMarkets.AwayOver1].Add((pred.AwayOver1, match.AwayBookings > 0));
                samples[BookingsMarkets.AwayOver2].Add((pred.AwayOver2, match.AwayBookings > 1));
                samples[BookingsMarkets.AwayOver3].Add((pred.AwayOver3, match.AwayBookings > 2));
            }

            var calibrators = new Dictionary<string, ConfidenceCalibrator>();
            var rows = new List<(string market, int n, double rawAvg, double actualRate, double calibratedAvg)>();

            foreach (var market in BookingsMarkets.All)
            {
                var list = samples[market];
                var calibrator = new ConfidenceCalibrator();
                calibrator.Fit(list);
                calibrators[market] = calibrator;

                if (list.Count == 0) continue;
                double rawAvg = list.Average(s => s.rawPct);
                double actualRate = list.Count(s => s.won) / (double)list.Count * 100;
                double calibratedAvg = list.Average(s => calibrator.Calibrate(s.rawPct));
                rows.Add((market, list.Count, rawAvg, actualRate, calibratedAvg));
            }

            return new BookingsBacktestSummary
            {
                From = from,
                To = to,
                MatchesAnalysed = analysed,
                Calibrators = calibrators,
                Report = GenerateReport(from, to, analysed, rows)
            };
        }

        private string GenerateReport(DateTime from, DateTime to, int analysed,
            List<(string market, int n, double rawAvg, double actualRate, double calibratedAvg)> rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine("══════════════════════════════════════════════════════════════");
            sb.AppendLine("  BOOKINGS CONFIDENCE CALIBRATION REPORT");
            sb.AppendLine("══════════════════════════════════════════════════════════════");
            sb.AppendLine($"  Period:    {from:yyyy-MM-dd}  →  {to:yyyy-MM-dd}");
            sb.AppendLine($"  Matches:   {analysed}");
            sb.AppendLine();
            sb.AppendLine("  Market            |    n | RawPred% | Actual% | Miscalib | Calibrated%");
            sb.AppendLine("  ────────────────────────────────────────────────────────────────────");

            foreach (var r in rows.OrderBy(r => r.market))
            {
                double miscalib = (r.actualRate - r.rawAvg) / 100.0;
                sb.AppendLine($"  {r.market,-18} | {r.n,4} | {r.rawAvg,7:F1}% | {r.actualRate,6:F1}% | {miscalib,8:F2} | {r.calibratedAvg,10:F1}%");
            }

            sb.AppendLine();
            sb.AppendLine("  NOTE: RawPred% is the engine's uncalibrated hit-rate average;");
            sb.AppendLine("        Actual% is what really happened; Miscalibration close to");
            sb.AppendLine("        0 means the raw rate is already trustworthy. Calibrated%");
            sb.AppendLine("        is what the fitted calibrator now outputs for that same");
            sb.AppendLine("        sample — it should track Actual% far more closely.");
            sb.AppendLine("══════════════════════════════════════════════════════════════");

            return sb.ToString();
        }
    }
}
