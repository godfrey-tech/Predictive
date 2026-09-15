using System.Text;
using ConsoleApp1.Mapper;
using ConsoleApp1.Modals;
using ConsoleApp1.Services;
using Predictive.Bookings.Implementations;
using Predictive.Bookings.Integrations.SportMonks;
using Predictive.Bookings.Modals;

// ============================================================
//  Predictive.Bookings — focused daily runner for the advanced
//  cards/bookings engine. Kept free of the other markets
//  (goals/corners/1X2/HT-FT) that ConsoleApp1 still runs as-is.
//
//  Runs today's fixtures across the 6 leagues covered by the current
//  SportMonks plan (Premier League, Championship, Bundesliga, Ligue 1,
//  Serie A, La Liga) — Championship added back 2026-09-13 via the
//  Extra Leagues add-on (currently trialing, 3 seasons of history).
//  See docs/HTFT.md's sibling plan doc for the phased roadmap.
// ============================================================

var leagues = new List<LeagueConfig>
{
    new("Premier League", 8, "Premier League"),
    new("Championship", 9, "Championship"),
    new("Bundesliga", 82, "Bundesliga"),
    new("League One France", 301, "Ligue 1"),
    new("Serie A", 384, "Serie A"),
    new("La Liga", 564, "La Liga"),
};

// Target date for today's fixture predictions — the user's LOCAL day, not UTC.
// DateTime.UtcNow.Date lags local date by several hours right after local midnight
// (e.g. at 00:03 SAST/UTC+2, UTC is still 22:03 the previous day), which would
// silently generate "today's" predictions for what the user considers yesterday.
// The accuracy tracker checks *previously logged* predictions regardless of this
// value, so this only controls which fixtures get newly predicted/logged this run.
DateTime targetDate = DateTime.Now.Date;

var historicalService = new EPLHistoricalService();
var smClient = new SportMonksClient();
var smHistorical = new SportMonksHistoricalService(smClient);
var fixtureResolver = new SportMonksFixtureResolver(smClient);
var refereeProvider = new SportMonksRefereeProvider(smClient);
var tracker = new PredictionTracker(ResolveResultsPath("PredictionLog.csv"), smClient);

await ReportTrackerProgressAsync(tracker);

bool RUN_BOOKINGS_BACKTEST = false;
if (RUN_BOOKINGS_BACKTEST)
    await RunCalibrationBacktestsAsync(leagues, historicalService);

await RunDailyPredictionsAsync(leagues, targetDate, historicalService, smHistorical, fixtureResolver, refereeProvider, tracker);

// ════════════════════════════════════════════════════════════
//  Phase implementations
// ════════════════════════════════════════════════════════════

// Backfills actual results for any prior predictions whose fixtures have since been
// played, then reports how the model's live track record looks so far — turns the
// one-off manual accuracy check from 2026-09-13 into a running, automatic log.
static async Task ReportTrackerProgressAsync(PredictionTracker tracker)
{
    int newlyChecked = await tracker.CheckPendingResultsAsync();
    Console.WriteLine($"[Tracker] Checked {newlyChecked} newly-completed fixture(s) against actual results.");
    Console.WriteLine(tracker.GenerateSummary());
}

// Per-league calibration check — validates whether BookingsEngine's raw hit-rates
// hold up outside Championship (the only league Phase 4's backtest ever covered).
// Full-season baseline vs. early-season-only (first 30 days from that season's
// Aug 1 start) tests whether cards specifically run lower early in a season, which
// a full-season average backtest would mask.
static async Task RunCalibrationBacktestsAsync(List<LeagueConfig> leagues, EPLHistoricalService historicalService)
{
    foreach (var league in leagues)
    {
        var historicalData = historicalService.LoadAndProcessCompetitionData(league.CsvKey);
        List<SeasonMatchRecord> matches = MatchMapper.MapToSeasonMatchRecords(historicalData);

        var backtester = new BookingsBacktester(matches);
        var fullSummary = backtester.Run(new DateTime(2022, 8, 1), new DateTime(2025, 6, 1));
        var earlySummary = backtester.Run(new DateTime(2022, 8, 1), new DateTime(2025, 6, 1), maxDaysIntoSeason: 30);

        string combinedReport = $"══════════ {league.DisplayName}: FULL SEASON ══════════\n{fullSummary.Report}\n" +
                                 $"══════════ {league.DisplayName}: FIRST 30 DAYS OF SEASON ONLY ══════════\n{earlySummary.Report}";

        Console.WriteLine(combinedReport);

        string backtestPath = ResolveResultsPath($"BookingsBacktest_{league.FileSafeName}_{DateTime.Now:yyyy-MM-dd}.txt");
        await File.WriteAllTextAsync(backtestPath, combinedReport);
    }
}

// Builds today's bookings predictions for every league and saves two parallel
// outputs: the normal one (referee bias applied) and a NoRefereeBias twin using
// the exact same engine/fixtures/date — diffing the two isolates the referee
// effect cleanly. Also logs every prediction to the accuracy tracker.
static async Task RunDailyPredictionsAsync(
    List<LeagueConfig> leagues, DateTime targetDate, EPLHistoricalService historicalService,
    SportMonksHistoricalService smHistorical, SportMonksFixtureResolver fixtureResolver,
    SportMonksRefereeProvider refereeProvider, PredictionTracker tracker)
{
    var output = new StringBuilder();
    var noRefereeOutput = new StringBuilder();

    foreach (var league in leagues)
    {
        var bookingsEngine = await BuildBookingsEngineAsync(league, historicalService, smHistorical);
        var fixturesForDate = await fixtureResolver.GetFixturesForDate(league.SportMonksLeagueId, targetDate);

        output.AppendLine($"══════════ {league.DisplayName} ({fixturesForDate.Count} fixture(s) on {targetDate:yyyy-MM-dd}) ══════════");
        output.AppendLine();
        noRefereeOutput.AppendLine($"══════════ {league.DisplayName} ({fixturesForDate.Count} fixture(s) on {targetDate:yyyy-MM-dd}) — NO REFEREE BIAS ══════════");
        noRefereeOutput.AppendLine();

        foreach (var (fixtureId, homeTeam, awayTeam) in fixturesForDate)
        {
            string? referee = await refereeProvider.GetRefereeNameForFixture(fixtureId);

            output.AppendLine($"{homeTeam} vs {awayTeam}" + (referee != null ? $"  (Referee: {referee})" : "  (Referee: unknown)"));
            output.AppendLine(bookingsEngine.Analyse(homeTeam, awayTeam, referee, asOf: targetDate));

            noRefereeOutput.AppendLine($"{homeTeam} vs {awayTeam}  (Referee bias not applied)");
            noRefereeOutput.AppendLine(bookingsEngine.Analyse(homeTeam, awayTeam, referee: null, asOf: targetDate));

            var pred = bookingsEngine.Predict(homeTeam, awayTeam, referee, asOf: targetDate);
            tracker.LogPrediction(new PredictionLogEntry
            {
                Date = targetDate,
                League = league.DisplayName,
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

    string noRefereeOutputPath = ResolveResultsPath($"SportMonksPredictions_{targetDate:yyyy-MM-dd}_NoRefereeBias.txt");
    await File.WriteAllTextAsync(noRefereeOutputPath, noRefereeOutput.ToString());
    Console.WriteLine($"[Comparison] No-referee-bias version saved to {Path.GetFileName(noRefereeOutputPath)} for diffing against the normal file.");
}

// Referee bias is sourced from SportMonks for every league (not CSV) — SportMonks
// has referee data uniformly across all 5 leagues, whereas football-data.co.uk's
// CSVs only carry a Referee column for English leagues, which would leave
// Bundesliga/Ligue 1/Serie A/La Liga with no historical sample at all (always
// neutral bias). Cached to disk for 7 days so this isn't refetched every run.
// Card/corner rate calculations still come from CSV, which has far deeper history.
static async Task<BookingsEngine> BuildBookingsEngineAsync(
    LeagueConfig league, EPLHistoricalService historicalService, SportMonksHistoricalService smHistorical)
{
    var historicalData = historicalService.LoadAndProcessCompetitionData(league.CsvKey);
    List<SeasonMatchRecord> matches = MatchMapper.MapToSeasonMatchRecords(historicalData);

    var refereeData = await smHistorical.LoadAllSeasonsCached(
        league.SportMonksLeagueId, league.CsvKey,
        ResolveResultsPath($"SportMonksRefereeHistory_{league.FileSafeName}.csv"), TimeSpan.FromDays(7));

    return new BookingsEngine(matches, new RefereeService(refereeData));
}

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

// csvKey must match EPLHistoricalService.LoadAndProcessCompetitionData's dictionary;
// sportMonksLeagueId confirmed against /v3/football/leagues for this SportMonks plan.
record LeagueConfig(string CsvKey, int SportMonksLeagueId, string DisplayName)
{
    public string FileSafeName => DisplayName.Replace(" ", "");
}
