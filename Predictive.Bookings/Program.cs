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

var historicalService = new EPLHistoricalService();
var championshipData = historicalService.LoadAndProcessCompetitionData("Championship");
List<SeasonMatchRecord> seasonMatches = MatchMapper.MapToSeasonMatchRecords(championshipData);

var refereeService = new RefereeService(seasonMatches);
var bookingsEngine = new BookingsEngine(seasonMatches, refereeService);

var todaysFixtures = new List<(string homeTeam, string awayTeam)>
{
    ("Birmingham", "Ipswich"),
};

string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
string outputPath = Path.Combine("Data", "Results", $"BookingsPredictions_{currentDate}.txt");
Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

foreach (var (homeTeam, awayTeam) in todaysFixtures)
{
    string output = $"{homeTeam} vs {awayTeam}\n" + bookingsEngine.Analyse(homeTeam, awayTeam) + "\n";
    Console.WriteLine(output);
    await File.AppendAllTextAsync(outputPath, output);
}
