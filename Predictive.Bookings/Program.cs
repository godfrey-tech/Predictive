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

// Target date for fixtures — yesterday, so predictions can be checked against
// results that have actually been played (today's fixtures haven't kicked off yet).
// Change to DateTime.UtcNow.Date for the normal daily run.
DateTime targetDate = DateTime.UtcNow.Date.AddDays(-1);

var smClient = new SportMonksClient();
var fixtureResolver = new SportMonksFixtureResolver(smClient);
var refereeProvider = new SportMonksRefereeProvider(smClient);

var output = new StringBuilder();
var historicalService = new EPLHistoricalService();

foreach (var (csvKey, leagueId, displayName) in leagues)
{
    var historicalData = historicalService.LoadAndProcessCompetitionData(csvKey);
    List<SeasonMatchRecord> matches = MatchMapper.MapToSeasonMatchRecords(historicalData);
    var refereeService = new RefereeService(matches);
    var bookingsEngine = new BookingsEngine(matches, refereeService);

    var fixturesForDate = await fixtureResolver.GetFixturesForDate(leagueId, targetDate);

    output.AppendLine($"══════════ {displayName} ({fixturesForDate.Count} fixture(s) on {targetDate:yyyy-MM-dd}) ══════════");
    output.AppendLine();

    foreach (var (fixtureId, homeTeam, awayTeam) in fixturesForDate)
    {
        string? referee = await refereeProvider.GetRefereeNameForFixture(fixtureId);

        output.AppendLine($"{homeTeam} vs {awayTeam}" + (referee != null ? $"  (Referee: {referee})" : "  (Referee: unknown)"));
        output.AppendLine(bookingsEngine.Analyse(homeTeam, awayTeam, referee, asOf: targetDate));
    }
}

string outputPath = ResolveResultsPath($"SportMonksPredictions_{targetDate:yyyy-MM-dd}.txt");
string result = output.ToString();
Console.WriteLine(result);
await File.WriteAllTextAsync(outputPath, result);
