using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Predictive.Bookings.Integrations.SportMonks;
using Predictive.Bookings.Modals;

namespace Predictive.Bookings.Implementations
{
    // Turns the one-off "did yesterday's predictions actually hit?" investigation
    // (done by hand on 2026-09-13) into a repeatable log: every prediction gets
    // written to Data/Results/PredictionLog.csv, and CheckPendingResults fills in
    // the actual outcome once a fixture has been played, using the same SportMonks
    // statistics (type_id 84=Yellowcards, 83=Redcards) that were used manually.
    public class PredictionTracker
    {
        private const int RedCardsTypeId = 83;
        private const int YellowCardsTypeId = 84;

        private readonly string _logPath;
        private readonly SportMonksClient _client;

        public PredictionTracker(string logPath, SportMonksClient client)
        {
            _logPath = logPath;
            _client = client;
        }

        public void LogPrediction(PredictionLogEntry entry)
        {
            bool writeHeader = !File.Exists(_logPath);
            using var writer = new StreamWriter(_logPath, append: true);
            if (writeHeader) writer.WriteLine(Header);
            writer.WriteLine(ToCsvLine(entry));
        }

        public List<PredictionLogEntry> LoadAll()
        {
            if (!File.Exists(_logPath)) return new List<PredictionLogEntry>();

            return File.ReadAllLines(_logPath)
                .Skip(1) // header
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(FromCsvLine)
                .ToList();
        }

        // Fetches the actual result for every logged prediction whose fixture is
        // in the past and hasn't been checked yet, then rewrites the log with those
        // filled in. Safe to call every run — already-checked rows are left alone.
        public async Task<int> CheckPendingResultsAsync()
        {
            var all = LoadAll();
            var pending = all.Where(e => e.ActualTotalCards == null && e.Date.Date < DateTime.UtcNow.Date).ToList();
            if (pending.Count == 0) return 0;

            int checkedCount = 0;
            foreach (var entry in pending)
            {
                var actual = await GetActualTotalCards(entry.FixtureId);
                if (actual.HasValue)
                {
                    entry.ActualTotalCards = actual;
                    checkedCount++;
                }
            }

            RewriteAll(all);
            return checkedCount;
        }

        private async Task<int?> GetActualTotalCards(int fixtureId)
        {
            var doc = await _client.GetAsync($"football/fixtures/{fixtureId}?include=statistics.type");
            if (!doc.RootElement.TryGetProperty("data", out var data)) return null;
            if (data.GetProperty("state_id").GetInt32() != 5) return null; // not finished yet
            if (!data.TryGetProperty("statistics", out var stats)) return null;

            int total = 0;
            foreach (var s in stats.EnumerateArray())
            {
                int typeId = s.GetProperty("type_id").GetInt32();
                if (typeId == RedCardsTypeId || typeId == YellowCardsTypeId)
                    total += s.GetProperty("data").GetProperty("value").GetInt32();
            }
            return total;
        }

        public string GenerateSummary()
        {
            var checkedEntries = LoadAll().Where(e => e.ActualTotalCards.HasValue).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("══════════════════════════════════════════════════════════════");
            sb.AppendLine("  LIVE PREDICTION ACCURACY TRACKER");
            sb.AppendLine("══════════════════════════════════════════════════════════════");
            sb.AppendLine($"  Checked fixtures so far: {checkedEntries.Count}");

            if (checkedEntries.Count == 0)
            {
                sb.AppendLine("  No checked results yet.");
                return sb.ToString();
            }

            sb.AppendLine();
            sb.AppendLine("  Market  |    n | AvgPred% | Actual% | Gap");
            sb.AppendLine("  ─────────────────────────────────────────");
            AppendMarketRow(sb, "Over 3", checkedEntries, e => e.FtOver3Pred, e => e.ActualTotalCards! > 2);
            AppendMarketRow(sb, "Over 4", checkedEntries, e => e.FtOver4Pred, e => e.ActualTotalCards! > 3);
            AppendMarketRow(sb, "Over 5", checkedEntries, e => e.FtOver5Pred, e => e.ActualTotalCards! > 4);
            AppendMarketRow(sb, "Over 6", checkedEntries, e => e.FtOver6Pred, e => e.ActualTotalCards! > 5);
            sb.AppendLine("══════════════════════════════════════════════════════════════");
            return sb.ToString();
        }

        private static void AppendMarketRow(StringBuilder sb, string label, List<PredictionLogEntry> entries,
            Func<PredictionLogEntry, double> predSelector, Func<PredictionLogEntry, bool> wonSelector)
        {
            double avgPred = entries.Average(predSelector);
            double actualRate = entries.Count(e => wonSelector(e)) / (double)entries.Count * 100;
            sb.AppendLine($"  {label,-7} | {entries.Count,4} | {avgPred,7:F1}% | {actualRate,6:F1}% | {actualRate - avgPred,+5:F1}");
        }

        private const string Header = "Date,League,HomeTeam,AwayTeam,FixtureId,Referee,FtOver3Pred,FtOver4Pred,FtOver5Pred,FtOver6Pred,ActualTotalCards";

        private static string ToCsvLine(PredictionLogEntry e) => string.Join(",",
            e.Date.ToString("yyyy-MM-dd"), Escape(e.League), Escape(e.HomeTeam), Escape(e.AwayTeam),
            e.FixtureId, Escape(e.Referee),
            e.FtOver3Pred.ToString(CultureInfo.InvariantCulture), e.FtOver4Pred.ToString(CultureInfo.InvariantCulture),
            e.FtOver5Pred.ToString(CultureInfo.InvariantCulture), e.FtOver6Pred.ToString(CultureInfo.InvariantCulture),
            e.ActualTotalCards?.ToString(CultureInfo.InvariantCulture) ?? "");

        private static string Escape(string value) => value.Contains(',') ? $"\"{value}\"" : value;

        private static PredictionLogEntry FromCsvLine(string line)
        {
            var parts = line.Split(',');
            return new PredictionLogEntry
            {
                Date = DateTime.Parse(parts[0], CultureInfo.InvariantCulture),
                League = parts[1],
                HomeTeam = parts[2],
                AwayTeam = parts[3],
                FixtureId = int.Parse(parts[4]),
                Referee = parts[5],
                FtOver3Pred = double.Parse(parts[6], CultureInfo.InvariantCulture),
                FtOver4Pred = double.Parse(parts[7], CultureInfo.InvariantCulture),
                FtOver5Pred = double.Parse(parts[8], CultureInfo.InvariantCulture),
                FtOver6Pred = double.Parse(parts[9], CultureInfo.InvariantCulture),
                ActualTotalCards = parts.Length > 10 && !string.IsNullOrEmpty(parts[10]) ? int.Parse(parts[10]) : null
            };
        }

        private void RewriteAll(List<PredictionLogEntry> entries)
        {
            using var writer = new StreamWriter(_logPath, append: false);
            writer.WriteLine(Header);
            foreach (var e in entries) writer.WriteLine(ToCsvLine(e));
        }
    }
}
