using System.Text;
using ConsoleApp1.Mapper;
using ConsoleApp1.Modals;
using ConsoleApp1.Services;
using Predictive.Bookings.Implementations;
using Predictive.Bookings.Integrations.SportMonks;

// ============================================================
//  Predictive.Bookings — focused daily runner for the advanced
//  cards/bookings engine. Kept free of the other markets
//  (goals/corners/1X2/HT-FT) that ConsoleApp1 still runs as-is.
//
//  Runs today's fixtures across the 5 leagues covered by the current
//  SportMonks plan (Premier League, Bundesliga, Ligue 1, Serie A,
//  La Liga) — Championship is on hold until the plan covers it.
//  See docs/HTFT.md's sibling plan doc for the phased roadmap.
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

// (csvCompetitionKey, SportMonks league id, display name) — csvCompetitionKey must
// match EPLHistoricalService.LoadAndProcessCompetitionData's dictionary; league id
// confirmed against /v3/football/leagues for this SportMonks plan.
var leagues = new List<(string csvKey, int sportMonksLeagueId, string displayName)>
{
    ("Premier League", 8, "Premier League"),
    ("Bundesliga", 82, "Bundesliga"),
    ("League One France", 301, "Ligue 1"),
    ("Serie A", 384, "Serie A"),
    ("La Liga", 564, "La Liga"),
};

// Target date for today's fixture predictions. The accuracy tracker below checks
// *previously logged* predictions against actual results regardless of this value,
// so this only controls which fixtures get newly predicted/logged this run.
DateTime targetDate = DateTime.UtcNow.Date;

var smClient = new SportMonksClient();
var fixtureResolver = new SportMonksFixtureResolver(smClient);
var refereeProvider = new SportMonksRefereeProvider(smClient);
var tracker = new PredictionTracker(ResolveResultsPath("PredictionLog.csv"), smClient);
var smHistorical = new SportMonksHistoricalService(smClient);

var output = new StringBuilder();
var historicalService = new EPLHistoricalService();

// Backfill actual results for any prior predictions whose fixtures have since been
// played, then report how the model's live track record looks so far — turns the
// one-off manual accuracy check from 2026-09-13 into a running, automatic log.
int newlyChecked = await tracker.CheckPendingResultsAsync();
Console.WriteLine($"[Tracker] Checked {newlyChecked} newly-completed fixture(s) against actual results.");
Console.WriteLine(tracker.GenerateSummary());

// Per-league calibration check — validates whether BookingsEngine's raw hit-rates
// hold up outside Championship (the only league Phase 4's backtest ever covered).
bool RUN_BOOKINGS_BACKTEST = false;
if (RUN_BOOKINGS_BACKTEST)
{
    foreach (var (csvKey, _, displayName) in leagues)
    {
        var historicalData = historicalService.LoadAndProcessCompetitionData(csvKey);
        List<SeasonMatchRecord> matches = MatchMapper.MapToSeasonMatchRecords(historicalData);

        var backtester = new BookingsBacktester(matches);

        // Full-season baseline vs. early-season-only (first 30 days from that
        // season's Aug 1 start) — tests whether cards specifically run lower early
        // in a season, which a full-season average backtest would mask.
        var fullSummary = backtester.Run(new DateTime(2022, 8, 1), new DateTime(2025, 6, 1));
        var earlySummary = backtester.Run(new DateTime(2022, 8, 1), new DateTime(2025, 6, 1), maxDaysIntoSeason: 30);

        string combinedReport = $"══════════ {displayName}: FULL SEASON ══════════\n{fullSummary.Report}\n" +
                                 $"══════════ {displayName}: FIRST 30 DAYS OF SEASON ONLY ══════════\n{earlySummary.Report}";

        Console.WriteLine(combinedReport);

        string backtestPath = ResolveResultsPath($"BookingsBacktest_{displayName.Replace(" ", "")}_{DateTime.Now:yyyy-MM-dd}.txt");
        await File.WriteAllTextAsync(backtestPath, combinedReport);
    }
}

foreach (var (csvKey, leagueId, displayName) in leagues)
{
    var historicalData = historicalService.LoadAndProcessCompetitionData(csvKey);
    List<SeasonMatchRecord> matches = MatchMapper.MapToSeasonMatchRecords(historicalData);

    // Referee bias is sourced from SportMonks for every league (not CSV) — SportMonks
    // has referee data uniformly across all 5 leagues, whereas football-data.co.uk's
    // CSVs only carry a Referee column for English leagues, which would leave
    // Bundesliga/Ligue 1/Serie A/La Liga with no historical sample at all (always
    // neutral bias). Cached to disk for 7 days so this isn't refetched every run.
    var refereeData = await smHistorical.LoadAllSeasonsCached(
        leagueId, csvKey, ResolveResultsPath($"SportMonksRefereeHistory_{displayName.Replace(" ", "")}.csv"), TimeSpan.FromDays(7));

    var refereeService = new RefereeService(refereeData);
    var bookingsEngine = new BookingsEngine(matches, refereeService);

    var fixturesForDate = await fixtureResolver.GetFixturesForDate(leagueId, targetDate);

    output.AppendLine($"══════════ {displayName} ({fixturesForDate.Count} fixture(s) on {targetDate:yyyy-MM-dd}) ══════════");
    output.AppendLine();

    foreach (var (fixtureId, homeTeam, awayTeam) in fixturesForDate)
    {
        string? referee = await refereeProvider.GetRefereeNameForFixture(fixtureId);

        output.AppendLine($"{homeTeam} vs {awayTeam}" + (referee != null ? $"  (Referee: {referee})" : "  (Referee: unknown)"));
        output.AppendLine(bookingsEngine.Analyse(homeTeam, awayTeam, referee, asOf: targetDate));

        var pred = bookingsEngine.Predict(homeTeam, awayTeam, referee, asOf: targetDate);
        tracker.LogPrediction(new Predictive.Bookings.Modals.PredictionLogEntry
        {
            Date = targetDate,
            League = displayName,
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            FixtureId = fixtureId,
            Referee = referee ?? "",
            FtOver3Pred = pred.FtOver3,
            FtOver4Pred = pred.FtOver4,
            FtOver5Pred = pred.FtOver5,
            FtOver6Pred = pred.FtOver6
        });
    }
}

string outputPath = ResolveResultsPath($"SportMonksPredictions_{targetDate:yyyy-MM-dd}.txt");
string result = output.ToString();
Console.WriteLine(result);
await File.WriteAllTextAsync(outputPath, result);

