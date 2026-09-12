using ConsoleApp1.Mapper;
using ConsoleApp1.Modals;
using ConsoleApp1.Services;
using Predictive.Bookings.Implementations;

// ============================================================
//  Predictive.Bookings — focused daily runner for the advanced
//  cards/bookings engine. Kept free of the other markets
//  (goals/corners/1X2/HT-FT) that ConsoleApp1 still runs as-is.
//  See docs/HTFT.md's sibling plan doc for the phased roadmap
//  (referee bias, real odds/EV, calibration, accumulators,
//  SportMonks) this project is meant to grow into.
// ============================================================

// Resolves Data/Results relative to the repo root by walking up from the running
// assembly — same fix as EPLHistoricalService.ResolveDataRoot(), so output lands in
// the right place (C:\Predictive - Version2\Data\Results) regardless of whether this
// runs via `dotnet run`, Visual Studio, or a published exe, instead of wherever the
// current working directory happens to be.
static string ResolveResultsPath(string fileName)
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir != null)
    {
        // Anchor on Data/England Football specifically (same signal EPLHistoricalService
        // uses) rather than a bare "Data" folder — a bare check can false-positive on a
        // stray Data/Results directory a previous buggy run left behind in bin output.
        if (Directory.Exists(Path.Combine(dir.FullName, "Data", "England Football")))
        {
            var resultsDir = Path.Combine(dir.FullName, "Data", "Results");
            Directory.CreateDirectory(resultsDir);
            return Path.Combine(resultsDir, fileName);
        }
        dir = dir.Parent;
    }
    throw new DirectoryNotFoundException($"Could not locate 'Data/England Football' by walking up from {AppContext.BaseDirectory}");
}

var historicalService = new EPLHistoricalService();
var championshipData = historicalService.LoadAndProcessCompetitionData("Championship");
List<SeasonMatchRecord> seasonMatches = MatchMapper.MapToSeasonMatchRecords(championshipData);

var refereeService = new RefereeService(seasonMatches);

bool RUN_BOOKINGS_BACKTEST = false;
Dictionary<string, ConfidenceCalibrator>? calibrators = null;

if (RUN_BOOKINGS_BACKTEST)
{
    var backtester = new BookingsBacktester(seasonMatches);
    var summary = backtester.Run(new DateTime(2022, 8, 1), new DateTime(2024, 6, 1));
    Console.WriteLine(summary.Report);

    string backtestPath = ResolveResultsPath($"BookingsBacktest_{DateTime.Now:yyyy-MM-dd}.txt");
    await File.WriteAllTextAsync(backtestPath, summary.Report);

    calibrators = summary.Calibrators;
}

var bookingsEngine = new BookingsEngine(seasonMatches, refereeService, oddsProvider: null, calibrators: calibrators);

var todaysFixtures = new List<(string homeTeam, string awayTeam)>
{
    ("Blackburn", "Millwall"),
    ("Charlton", "Portsmouth"),
    //("Coventry", "Hull"),
    //("Southampton", "Wrexham"),
    //("Middlesbrough", "Swansea"),
    //("Norwich", "Millwall"),
};

string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
string outputPath = ResolveResultsPath($"BookingsPredictions_{currentDate}.txt");

foreach (var (homeTeam, awayTeam) in todaysFixtures)
{
    string output = $"{homeTeam} vs {awayTeam}\n" + bookingsEngine.Analyse(homeTeam, awayTeam) + "\n";
    Console.WriteLine(output);
    await File.AppendAllTextAsync(outputPath, output);
}

bool RUN_CARD_ACCUMULATORS = false;
if (RUN_CARD_ACCUMULATORS)
{
    var accumulatorBuilder = new CardAccumulatorBuilder(bookingsEngine);
    var topAccumulators = accumulatorBuilder.BuildTopAccumulators(todaysFixtures);

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("CARD ACCUMULATORS");
    foreach (var acc in topAccumulators)
    {
        sb.AppendLine(string.Join(" + ", acc.Legs.Select(l => $"{l.HomeTeam} v {l.AwayTeam}: {l.Market} ({l.Probability * 100:F1}%)")));
        string oddsText = acc.CombinedOdds.HasValue ? $"@{acc.CombinedOdds:F2}" : "(no odds available)";
        string evText = acc.ExpectedValue.HasValue ? $"EV {acc.ExpectedValue * 100:F1}%, Kelly {acc.KellyFraction * 100:F1}% bankroll" : "";
        sb.AppendLine($"  Combined: {acc.CombinedProbability * 100:F1}% {oddsText} {evText}");
        sb.AppendLine();
    }

    string accumulatorOutput = sb.ToString();
    Console.WriteLine(accumulatorOutput);
    await File.AppendAllTextAsync(outputPath, accumulatorOutput);
}
