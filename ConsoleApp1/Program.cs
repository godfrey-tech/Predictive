using ConsoleApp1.Implementations;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Mapper;
using ConsoleApp1.Modals;
using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using ConsoleApp1.Services;
using CsvHelper;
using MathNet.Numerics.Distributions;
using Polly;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "b2bdd266bb9a43ada8d79555fabdd0d2"; // Replace with your API key
        string endpoint = "https://api.football-data.org/v4/matches"; // Replace with the desired endpoint

        EPLHistoricalService _ePLHistoricalService = new EPLHistoricalService();
        //_ePLHistoricalService.HistoricalMatchesFromCSVFiles();
        var championShipData = _ePLHistoricalService.LoadAndProcessChampionShipData();

        var data = _ePLHistoricalService.LoadAndProcessData();
        List<SeasonMatchRecord> seasonMatches = MatchMapper.MapToSeasonMatchRecords(championShipData);
        var distinctTeams = seasonMatches
                            .SelectMany(match => new[] { match.HomeTeam, match.AwayTeam })
                            .Distinct()
                            .OrderBy(team => team)
                            .ToList();
        
        ISeasonStatsService _season = new SeasonStatsService(seasonMatches);
        IHeadToHeadStatsService _headToHeadStatsService = new HeadToHeadStatsService(seasonMatches);
        IHeadToHeadStatsStreakService _headToHeadStreakStats = new HeadToHeadStatsStreakService(seasonMatches);
        var earliestDate = data.Min(x => x.Date);
        var latestDate = data.Max(x => x.Date);
        var startSeason = new DateTime(2019, 08, 1);
        var endSeason = new DateTime(2025, 09, 09);

        IMatchOutcomePredictor _matchOutcomePredictor = new MatchOutcomePredictor(_season, _headToHeadStatsService);
        var matches = new List<(string homeTeam, string awayTeam)>
        {
            //("Djurgarden", "Sirius"),
            ("Helsingborg", "Trelleborgs"),
            //("HamKam", "Rosenborg"),
            //("Tromso", "Stromsgodset"),
            //("Molde", "Valerenga"),
            //("Oxford", "Sheffield United"),
            //("Preston", "Bristol City"),
            //("Sheffield Weds", "QPR"),
            //("Southampton", "Middlesbrough"),
            //("Stoke", "Norwich"),
            //("Watford", "Hull"),
            //("Bromley", "Gillingham"),

            //("Viking", "Rosenborg"),
            //("Stromsgodset", "Molde"),
            //("Milton Keynes Dons", "Walsall"),
            //("Oldham", "Gillingham"),
            //("Shrewsbury", "Accrington"),
            //("Tranmere", "Notts County"),
            //("Wigan", "Stockport"),

            //("Dundalk", "Longford"),
            //("Finn Harps", "Bray"),

            //("Shamrock Rovers", "Derry City"),
            //("Molde", "Bryne"),
            //("Stromsgodset", "Haugesund"),
            //("Valerenga", "Sandefjord"),
            //("Sirius", "Oster"),
            //("Accrington", "Gillingham"),
            //("Barnet", "Fleetwood Town"),
            //("Bristol Rvs", "Harrogate"),
            //("Cambridge", "Cheltenham"),
            //("Chesterfield", "Barrow"),
            //("Colchester", "Tranmere"),
            //("Grimsby", "Crawley Town"),
            //("Milton Keynes Dons", "Oldham"),
            //("Newport County", "Notts County"),
            //("Salford", "Crewe"),
            //("Shrewsbury", "Bromley"),
            //("Walsall", "Swindon"),
            //("Bradford", "Wycombe"),
            //("Burton", "Mansfield"),
            //("Doncaster", "Exeter"),
            //("Doncaster", "Leyton Orient"),
            //("Lincoln", "Reading"),
            //("Plymouth", "Barnsley"),
            //("Rotherham", "Port Vale"),
            //("Wigan", "Northampton"),
            //("FC Tokyo", "Urawa Reds"),
            //("Shonan Bellmare", "Cerezo Osaka"),
            //("Rovaniemi","Inter Turku"),
            //("Widzew Lodz","Zaglebie"),
            //("Wisla Plock","Korona Kielce"),
            //("GKS Katowice", "Rakow"),
            //("KFUM Oslo", "Brann"),
            //("Molde", "Stromsgodset"),
            //("Viking", "Bodo/Glimt"),
            //("Djurgarden", "Elfsborg"),
            //("Oster", "Malmo FF"),
            //("Degerfors", "GAIS"),
            //("GAIS", "Hammarby"),
            //("Brommapojkarna", "Osters"),
            //("AIK", "Degerfors"),
            //("Varnamo", "Djurgarden"),
            //("Hacken", "Halmstad"),
            //("Bryne", "Valerenga"),
            //("Stromsgodset", "Tromso"),
            //("Rosenborg", "HamKam"),
            //("Kristiansund", "Sarpsborg 08"),
            //("Haugesund", "KFUM Oslo"),
            //("Brann", "Viking"),
            //("Sligo Rovers", "Derry City"),
            //("Goteborg", "Elfsborg"),
            //("Malmo FF", "Norrkoping"),
            //("Bodo/Glimt", "Sandefjord"),
            //("Fredrikstad", "Molde"),
            //("Iwata", "V-Varen Nagasaki"),
            //("Iwata", "V-Varen Nagasaki"),
            //("Goteborg", "Sirius"),
            //("Halmstad", "AIK"),
            //("Elfsborg", "Hacken"),
            //("Djurgarden", "Degerfors"),
            //("KFUM Oslo", "Bryne"),
            //("Sarpsborg 08", "Haugesund"),
            //("Sandefjord", "Rosenborg"),
            //("Charlotte", "Orlando City"),
            //("CF Montreal", "Inter Miami"),
            //("FC Cincinnati", "Chicago Fire"),
            //("DC United", "Atlanta Utd"),
            //("Austin FC", "Los Angeles FC"),
            //("Nashville SC", "Philadelphia Union"),
            //("Real Salt Lake", "St. Louis City"),
            //("San Jose Earthquakes", "New York Red Bulls"),
            //("Portland Timbers", "New England Revolution"),
            //("San Diego FC", "Houston Dynamo"),
            //("Seattle Sounders", "Columbus Crew"),
            //("GAIS", "Malmo FF"),
            //("Oster", "Mjallby"),
            //("Hammarby", "Varnamo"),
            //("HamKam", "Brann"),
            //("Viking", "Stromsgodset"),
            //("Valerenga", "Fredrikstad"),
            //("Kristiansund", "Bodo/Glimt"),
            //("Tromso", "Molde"),
            //("Sligo Rovers", "Shamrock Rovers"),
            //("Machida", "Shimizu S-Pulse"),
            //("Okayama", "Sanfrecce Hiroshima"),
            //("Kawasaki Frontale", "Kashima Antlers"),
            //("Kyoto", "Albirex Niigata"),
            //("Kashiwa Reysol", "FC Tokyo"),
            //("Nagoya Grampus", "Verdy"),
            //("Yokohama FC", "Yokohama F. Marinos"),
            //("Vissel Kobe", "Shonan Bellmare"),
            //("Cerezo Osaka", "Gamba Osaka"),
            //("New York City","Toronto FC"),
            //("Drogheda","Galway"),
            //("Derry City","Waterford"),
            //("St. Patricks","Bohemians"),
            //("Shelbourne","Cork City"),
            //("New York City","Toronto FC"),
            //("Vissel Kobe", "Sanfrecce Hiroshima"),
            //("Bohemians", "Sligo Rovers"),
            //("Galway", "Shelbourne"),
            //("Derry City", "Drogheda"),
            //("Cork City", "St. Patricks"),
            //("Shamrock Rovers", "Waterford"),
            //("Toronto FC", "New York City"),
            //("New England Revolution", "Nashville SC"),
            //("FC Dallas", "San Jose Earthquakes"),
            //("Chicago Fire", "Philadelphia Union"),
            //("St. Louis City", "Orlando City"),
            //("Sporting Kansas City", "Charlotte"),
            //("Minnesota United", "Houston Dynamo"),
            //("Colorado Rapids", "Los Angeles Galaxy"),
            //("Vancouver Whitecaps", "San Diego FC"),
            //("Bohemians", "Shamrock Rovers"),
            //("Cork City", "Drogheda"),
            //("Sligo Rovers", "Galway"),
            //("Waterford", "Shelbourne"),
            //("St. Patricks", "Derry City"),
            //("Cork City", "Shelbourne"),
            //("PK-35 Vantaa", "SJK"),
            //("Valerenga", "Molde"),
            //("HamKam", "Tromso"),
            //("Sarpsborg 08", "Bryne"),
            //("Kristiansund", "Rosenborg"),
            //("Sandefjord", "Haugesund"),
            //("KFUM Oslo", "Stromsgodset"),
            //("Viking", "Fredrikstad"),
            //("Cerezo Osaka", "Shimizu S-Pulse"),
            //("Shonan Bellmare", "Okayama"),
            //("Urawa Reds", "Yokohama FC"),
            //("Yokohama FC", "Sanfrecce Hiroshima"),
            //("Gamba Osaka", "FC Tokyo"),
            //("Nashville SC", "New York City"),
            //("Inter Miami", "Columbus Crew"),
            //("New York Red Bulls", "Atlanta Utd"),
            //("Toronto FC", "Charlotte"),
            //("FC Cincinnati", "DC United"),
            //("Orlando City", "Chicago Fire"),
            //("FC Dallas", "Philadelphia Union"),
            //("Houston Dynamo", "Sporting Kansas City"),
            //("San Jose Earthquakes", "Austin FC"),
            //("Stromsgodset", "HamKam"),
            //("Tromso", "Valerenga"),
            //("Rizespor", "Hatayspor"),
            //("Fenerbahce", "Konyaspor"),
            //("Adanaspor", "Gaziantep"),
            //("Alanyaspor", "Sivasspor"),
            //("Waterford", "Grasshoppers"),
            //("Sligo Rovers", "St. Patricks"),
            //("Bohemians", "Derry City"),
            //("Cork City", "Shelbourne"),
            //("Shamrock Rovers", "Galway"),
            //("Aarau", "Grasshoppers"),
            //("Kasimpasa", "Goztep"),
            //("Galatasaray", "Istanbulspor"),
            //("Antalyaspor", "Trabzonspor"),
            //("Fredrikstad", "Rosenborg"),
            //("Bodo/Glimt", "Viking"),
            //("Brann", "Molde"),
            //("Albirex Niigata", "Shonan Bellmare"),
            //("Yokohama F. Marinos", "Kashima Antlers"),
            //("Shimizu S-Pulse", "Vissel Kobe"),
            //("Yokohama FC", "Kashiwa Reysol"),
            //("Verdy", "Kyoto"),
            //("FC Tokyo", "Sanfrecce Hiroshima"),
            //("Kawasaki Frontale", "Gamba Osaka"),
            //("Seattle Sounders", "FC Dallas"),
            //("San Diego FC", "Los Angeles FC"),
            //("Gornik Zabrze", "Korona Kielce"),
            //("Polonia Warszawa", "Stal Mielec"),
            //("Jagiellonia", "Pogon Szczecin"),
            //("Puszcza", "Slask Wroclaw"),
            //("Lech Poznan", "Piast Gliwice"),
            //("Radomiak Radom", "Motor Lublin"),
            //("Lechia Gdansk", "GKS Katowice"),
            //("Zaglebie", "Cracovia"),
            //("Rakow", "Widzew Lodz"),
            //("Silkeborg", "Viborg"),
            //("Vejle", "Sonderjyske"),
            //("Lyngby", "Aalborg"),
            //("Dender", "Mechelen"),
            //("Westerlo", "Standard"),
            //("Charleroi", "Oud-Heverlee Leuven"),
            //("Salzburg", "SK Rapid"),
            //("Austria Vienna", "BW Linz"),
            //("Sturm Graz", "Wolfsberger AC"),
            //("Willem II", "Dordrecht"),
            //("Real Madrid", "Sociedad"),
            //("Leganes", "Valladolid"),
            //("Espanol", "Las Palmas"),
            //("Getafe", "Celta"),
            //("Alaves", "Osasuna"),
            //("Vallecano", "Mallorca"),
            //("Gaziantep", "Kasimpasa"),
            //("Goztep", "Galatasaray"),
            //("Sheffield United", "Sunderland"),
            //("Napoli", "Cagliari"),
            //("Napoli", "Cagliari"),
            //("Betis", "Valencia"),
            //("Livingston", "Partick"),
            //("Famalicao", "Casa Pia"),
            //("Rio Ave", "Gil Vicente"),
            //("Cambuur", "Den Haag")
            //("Aston Villa", "Tottenham"),
            //("Chelsea", "Man United")
            //("Servette", "Lugano"),
            //("Young Boys", "Luzern"),
            //("Djurgarden", "Mjallby"),
            //("Elfsborg", "Brommapojkarna"),
            //("Goteborg", "Osters"),
            //("Norrkoping", "Degerfors"),
            //("Varnamo", "Malmo FF"),
            //("Esbjerg", "Hvidovre IF"),
            //("Charlton", "Wycombe"),
            //("Osasuna", "Ath Madrid"),
            //("Vallecano", "Betis"),
            //("Espanol", "Barcelona"),
            //("Getafe", "Ath Bilbao")
            //("Willem II", "Zwolle"),
            //("PSV Eindhoven", "Heracles"),
            //("Nijmegen", "NAC Breda"),
            //("Go Ahead Eagles", "Heerenveen"),
            //("Feyenoord", "Waalwijk"),
            //("Utrecht", "Sparta Rotterdam"),
            //("Twente", "AZ Alkmaar"),
            //("Groningen", "Ajax"),
            //("Almere City", "For Sittard")
            //("Dunkerque", "Guingamp"),
            //("Bologna", "Milan"),
            //("Alaves", "Valencia"),
            //("Villarreal", "Leganes"),
            //("Real Madrid", "Mallorca"),
            //("Sunderland", "Coventry City"),
            //("Celta", "Sociedad"),
            //("Sevilla", "Las Palmas"),
            //("Valladolid", "Girona"),
            //("Din. Bucuresti", "FC Rapid Bucuresti"),
            //("River Plate", "Barracas Central"),
            //("Goteborg", "Djurgarden"),
            //("Vejle", "Lyngby"),
            //("Kayserispor", "Antalyaspor"),
            //("Den Haag", "Cambuur")
            //("Venezia", "Fiorentina"),
            //("Atalanta", "Roma"),
            //("Boavista", "Porto")
            //("Napoli", "Genoa"),
            //("Betis", "Osasuna"),
            //("Leganes", "Espanol"),
            //("Barcelona", "Real Madrid"),
            //("Ath Bilbao", "Alaves"),
            //("Leverkusen", "Dortmund"),
            //("Stuttgart", "Augsburg"),
            //("Udinese", "Monza"),
            //("Verona", "Lecce"),
            //("Torino", "Inter"),
            //("Newcastle", "Chelsea"),
            //("Nott'm Forest", "Leicester"),
            //("Man United", "West Ham"),
            //("Tottenham", "Crystal Palace"),
            //("Liverpool", "Arsenal"),
            //("Zwolle", "Go Ahead Eagles"),
            //("Twente", "Utrecht"),
            //("Feyenoord", "PSV Eindhoven"),
            //("Ajax", "Nijmegen"),
            //("Lazio", "Juventus"),
            //("AZ Alkmaar", "Groningen")
            //("Le Havre", "Marseille"),
            //("Auxerre", "Nantes"),
            //("Angers", "Strasbourg"),
            //("Monaco", "Lyon"),
            //("Montpellier", "Paris SG"),
            //("Brest", "Lille"),
            //("Rennes", "Nice"),
            //("Toulouse", "Lens")
            //("Union Berlin", "Heidenheim"),
            //("Werder Bremen", "RB Leipzig"),
            //("Bochum", "Mainz"),
            //("Bayern Munich", "M'gladbach"),
            //("Valencia", "Getafe"),
            //("Girona", "Villarreal"),
            //("Celta", "Sevilla"),
            //("Mallorca", "Valladolid"),
            //("Ath Madrid", "Sociedad"),
            //("Bournemouth", "Aston Villa"),
            //("Fulham", "Everton"),
            //("Ipswich", "Brentford"),
            //("Southampton", "Man City"),
            //("Wolves", "Brighton")
            //("Wolfsburg", "Hoffenheim"),
            //("Milan", "Bologna"),
            //("Coventry", "Sunderland")
            //("Willem II", "Heracles"),
            //("Gaziantep", "Alanyaspor")
            //("Buyuksehyr", "Fenerbahce")
            //("Arsenal", "Man City"),       // Arsenal vs Manchester City
            //("Chelsea", "Liverpool"),       // Chelsea vs Liverpool
            //("Tottenham", "Aston Villa"),   // Tottenham Hotspur vs Aston Villa
            //("Leicester", "West Ham"),      // Leicester City vs West Ham United
            //("Brighton", "Crystal Palace"), // Brighton & Hove Albion vs Crystal Palace
            //("Wolves", "Burnley"),   // Wolverhampton Wanderers vs Burnley
            //("Nott'm Forest", "Fulham"),// Nottingham Forest vs Fulham
            //("Southampton", "Brentford"),   // Southampton vs Brentford
            //("Bournemouth", "Sheffield United"), // Bournemouth vs Sheffield United
            //("Everton", "Man United")       // Everton vs Manchester United
        };
        string outputTest = "";
        foreach (var match in matches)
        {
            var predictedOutcome = _matchOutcomePredictor.PredictOutcome(match.homeTeam, match.awayTeam, startSeason, endSeason);

            outputTest = $"{match.homeTeam} vs {match.awayTeam}: {predictedOutcome.Outcome} - {predictedOutcome.Category}\n" +
                         $"Home:{Math.Round(predictedOutcome.HomeWinProbability, 2)}% vs Away:{Math.Round(predictedOutcome.AwayWinProbability, 2)}% vs Draw:{Math.Round(predictedOutcome.DrawProbability, 2)}% Diff: {Math.Round(predictedOutcome.DifferenceProbability, 2)}%\n" +
                         $"Home Av Gls: {Math.Round(predictedOutcome.HomeAverageGoalsScored, 2)} vs Away Av Gls: {Math.Round(predictedOutcome.AwayAverageGoalsScored, 2)}\n" +
                         $"Home Historical Av Gls: {Math.Round(predictedOutcome.HomeHistoricalAverageGoalsScored, 2)} vs Away Historical Av Gls: {Math.Round(predictedOutcome.AwayHistoricalAverageGoalsScored, 2)}\n" +
                         $"Over 1 Gls: {Math.Round(predictedOutcome.Over1GoalsProbability, 2)}% \n" +
                         $"Over 2 Gls: {Math.Round(predictedOutcome.Over2GoalsProbability, 2)}% \n" +
                         $"\n";

            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string filePathTest = $@"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\Results\Predictions_{currentDate}.txt";

            await File.AppendAllTextAsync(filePathTest, outputTest);
        }

        var dataFetcher = new DataFetcher();
        var apiResponse = await dataFetcher.GetUpcomingFixturesDataAsync(apiKey, endpoint);

        // Now you can access the matches
        foreach (var match in apiResponse.Matches)
        {
            if (match.Competition.Code == "PL" && match.Competition.Name == "Premier League")
            {
                var homeTeam = MatchMapper.GetOfficialTeamName(match.HomeTeam.Name);
                var awayTeam = MatchMapper.GetOfficialTeamName(match.AwayTeam.Name);

                if (homeTeam != null && awayTeam != null)
                {
                    var result = _matchOutcomePredictor.PredictOutcome(homeTeam, awayTeam, startSeason, endSeason);

                    Console.WriteLine($"Match: {match.HomeTeam.Name} vs {match.AwayTeam.Name}");
                    Console.WriteLine($"Date: {match.UtcDate:yyyy/MM/dd HH:mm:ss} - Competition: {match.Competition.Name} ({match.Competition.Type})");
                }
            }
        }
        //LiverPool
        var headToHead = _season.GetHeadToHeadMatches("Liverpool", "West Ham", startSeason, endSeason);
        int Homewins = _season.GetHeadToHeadWins("Liverpool", "West Ham", startSeason, endSeason);
        int Homedraw = _season.GetHeadToHeadDraw("Liverpool", "West Ham", startSeason, endSeason);
        int Homeloss = _season.GetHeadToHeadLosses("Liverpool", "West Ham", startSeason, endSeason);
        int totalBookings = _season.GetTotalBookings("Liverpool", startSeason, endSeason);
        int totalCorners = _season.GetTotalCorners("Liverpool", startSeason, endSeason);
        int totalDraws = _season.GetTotalDraw("Liverpool", startSeason, endSeason);
        int totalLosses = _season.GetTotalLosses("Liverpool", startSeason, endSeason);
        int totalWins = _season.GetTotalWins("Liverpool", startSeason, endSeason);
        int totalGoals = _season.GetTotalGoals("Liverpool", startSeason, endSeason);
        double averageBookings = _season.GetAverageBookings("Liverpool", startSeason, endSeason);
        double averageCorners = _season.GetAverageCorners("Liverpool", startSeason, endSeason);
        double averageGoalsScored = _season.GetAverageGoalsScored("Liverpool", startSeason, endSeason);

        //head to head streak stats
        int longestWinStreak = _headToHeadStreakStats.GetLongestWinStreak("Liverpool", "West Ham");
        int WlongestWinStreak = _headToHeadStreakStats.GetLongestWinStreak("West Ham", "Liverpool");


        //Arsenal
        var AheadToHead = _season.GetHeadToHeadMatches("Arsenal", "Liverpool", startSeason, endSeason);
        int AHomewins = _season.GetHeadToHeadWins("Arsenal", "Liverpool", startSeason, endSeason);
        int AHomedraw = _season.GetHeadToHeadDraw("Arsenal", "Liverpool", startSeason, endSeason);
        int AHomeloss = _season.GetHeadToHeadLosses("Arsenal", "Liverpool", startSeason, endSeason);
        int AtotalBookings = _season.GetTotalBookings("Arsenal", startSeason, endSeason);
        int AtotalCorners = _season.GetTotalCorners("Arsenal", startSeason, endSeason);
        int AtotalDraws = _season.GetTotalDraw("Arsenal", startSeason, endSeason);
        int AtotalLosses = _season.GetTotalLosses("Arsenal", startSeason, endSeason);
        int AtotalWins = _season.GetTotalWins("Arsenal", startSeason, endSeason);
        int AtotalGoals = _season.GetTotalGoals("Arsenal", startSeason, endSeason);
        double AaverageBookings = _season.GetAverageBookings("Arsenal", startSeason, endSeason);
        double AaverageCorners = _season.GetAverageCorners("Arsenal", startSeason, endSeason);
        double AaverageGoalsScored = _season.GetAverageGoalsScored("Arsenal", startSeason, endSeason);

        //West Ham
        var WheadToHead = _season.GetHeadToHeadMatches("West Ham", "Liverpool", startSeason, endSeason);
        int WHomewins = _season.GetHeadToHeadWins("West Ham", "Liverpool", startSeason, endSeason);
        int WHomedraw = _season.GetHeadToHeadDraw("West Ham", "Liverpool", startSeason, endSeason);
        int WHomeloss = _season.GetHeadToHeadLosses("West Ham", "Liverpool", startSeason, endSeason);
        int WtotalBookings = _season.GetTotalBookings("West Ham", startSeason, endSeason);
        int WtotalCorners = _season.GetTotalCorners("West Ham", startSeason, endSeason);
        int WtotalDraws = _season.GetTotalDraw("West Ham", startSeason, endSeason);
        int WtotalLosses = _season.GetTotalLosses("West Ham", startSeason, endSeason);
        int WtotalWins = _season.GetTotalWins("West Ham", startSeason, endSeason);
        int WtotalGoals = _season.GetTotalGoals("West Ham", startSeason, endSeason);
        double WaverageBookings = _season.GetAverageBookings("West Ham", startSeason, endSeason);
        double WaverageCorners = _season.GetAverageCorners("West Ham", startSeason, endSeason);
        double WaverageGoalsScored = _season.GetAverageGoalsScored("West Ham", startSeason, endSeason);




        var matchDataList2 = _ePLHistoricalService.PrepareMatchDataV2(data);
        var historicalMatch = _ePLHistoricalService.GetTeamStats(matchDataList2, "Liverpool", "West Ham");
        _ePLHistoricalService.FileOutputCsvV3(historicalMatch);

        //var _predictorService = new PredictorService();
        //var model2 = _predictorService.TrainModelV3(matchDataList2);
        //_predictorService.PredictMatchOutcomeV3(model2, "Bournemouth", "Liverpool", matchDataList2);

        //var matchDataList = _ePLHistoricalService.PrepareMatchData(data);
        //var model = _predictorService.TrainModelV2(matchDataList);
        //_predictorService.PredictMatchOutcomeV2(model, "Bournemouth", "Liverpool", matchDataList);


        apiResponse = await dataFetcher.GetUpcomingFixturesDataAsync(apiKey, endpoint);

        // Now you can access the matches
        foreach (var match in apiResponse.Matches)
        {
            Console.WriteLine($"Match: {match.HomeTeam.Name} vs {match.AwayTeam.Name}");
            Console.WriteLine($"Date: {match.UtcDate:yyyy/MM/dd HH:mm:ss} - Competition: {match.Competition.Name} ({match.Competition.Type})");

            // Get last 10 matches stats for the home team
            var last10HomeMatchesStats = await dataFetcher.GetLastNMatchesStats(match.HomeTeam.Name, 10, match.HomeTeam.Id, apiKey);
            Console.WriteLine($"Last 10 Matches for {match.HomeTeam.Name}: Wins: {last10HomeMatchesStats.Wins}, Losses: {last10HomeMatchesStats.Losses}, Draws: {last10HomeMatchesStats.Draws}, Total Goals: {last10HomeMatchesStats.TotalGoalsScored}, Total Home Goals: {last10HomeMatchesStats.TotalHomeGoalsScored}");
            Console.WriteLine($"Last 10 Matches for {match.HomeTeam.Name}: Wins: {last10HomeMatchesStats.HalfTimeWins}, Losses: {last10HomeMatchesStats.HalfTimeLosses}, Draws: {last10HomeMatchesStats.HalfTimeDraws}");

            // Get last 5 matches stats for the home team
            var last5HomeMatchesStats = await dataFetcher.GetLastNMatchesStats(match.HomeTeam.Name, 5, match.HomeTeam.Id, apiKey);
            Console.WriteLine($"Last 5 Matches for {match.HomeTeam.Name}: Wins: {last5HomeMatchesStats.Wins}, Losses: {last5HomeMatchesStats.Losses}, Draws: {last5HomeMatchesStats.Draws}, Total Goals: {last5HomeMatchesStats.TotalGoalsScored}, Total Home Goals: {last5HomeMatchesStats.TotalHomeGoalsScored}");
            Console.WriteLine($"Last 5 Matches for {match.HomeTeam.Name}: Wins: {last5HomeMatchesStats.HalfTimeWins}, Losses: {last5HomeMatchesStats.HalfTimeLosses}, Draws: {last5HomeMatchesStats.HalfTimeDraws}");

            // Get last 10 matches stats for the away team
            var last10AwayMatchesStats = await dataFetcher.GetLastNMatchesStats(match.AwayTeam.Name, 10, match.AwayTeam.Id, apiKey);
            Console.WriteLine($"Last 10 Matches for {match.AwayTeam.Name}: Wins: {last10AwayMatchesStats.Wins}, Losses: {last10AwayMatchesStats.Losses}, Draws: {last10AwayMatchesStats.Draws}, Total Goals: {last10AwayMatchesStats.TotalGoalsScored}, Total Away Goals: {last10AwayMatchesStats.TotalAwayGoalsScored}");
            Console.WriteLine($"Last 10 Matches for {match.AwayTeam.Name}: Wins: {last10AwayMatchesStats.HalfTimeWins}, Losses: {last10AwayMatchesStats.HalfTimeLosses}, Draws: {last10AwayMatchesStats.HalfTimeDraws}");

            // Get last 5 matches stats for the away team
            var last5AwayMatchesStats = await dataFetcher.GetLastNMatchesStats(match.AwayTeam.Name, 5, match.AwayTeam.Id, apiKey);
            Console.WriteLine($"Last 5 Matches for {match.AwayTeam.Name}: Wins: {last5AwayMatchesStats.Wins}, Losses: {last5AwayMatchesStats.Losses}, Draws: {last5AwayMatchesStats.Draws}, Total Goals: {last5AwayMatchesStats.TotalGoalsScored}, Total Away Goals: {last5AwayMatchesStats.TotalAwayGoalsScored}");
            Console.WriteLine($"Last 5 Matches for {match.AwayTeam.Name}: Wins: {last5AwayMatchesStats.HalfTimeWins}, Losses: {last5AwayMatchesStats.HalfTimeLosses}, Draws: {last5AwayMatchesStats.HalfTimeDraws}");

            // Fetch historical data for the fixture
            Console.WriteLine($"\nHistorical Matches between {match.HomeTeam.Name} and {match.AwayTeam.Name}:");

            var historicalData = await dataFetcher.GetHistoricalDataForFixture(match.HomeTeam.Name, match.AwayTeam.Name, match.HomeTeam.Id);

            // Display historical match results
            foreach (var historicalDataForFixture in historicalData)
            {
                Console.WriteLine($"{historicalDataForFixture.HomeTeam} vs {historicalDataForFixture.AwayTeam} on {historicalDataForFixture.MatchDate:yyyy/MM/dd} - Result: {historicalDataForFixture.Outcome} (Score: {historicalDataForFixture.HomeScore} - {historicalDataForFixture.AwayScore}) - Competition: {historicalDataForFixture.Competition}");
            }

            // Create an empty line for better readability
            Console.WriteLine();

            // Predict the outcome based on historical data
            var trainingData = historicalData.Select(match => new MatchData
            {
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                Outcome = match.Outcome,

                HomeWinsLast5 = last5HomeMatchesStats.Wins,
                HomeLossesLast5 = last5HomeMatchesStats.Losses,
                HomeDrawsLast5 = last5HomeMatchesStats.Draws,
                HomeAverageGoalsScoredLast5 = last5HomeMatchesStats.TotalGoalsScored / 5.0, // Avoid integer division
                                                                                            // HomeAverageGoalsConcededLast5 = last5HomeMatchesStats.TotalGoalsConceded / 5.0, // Avoid integer division

                AwayWinsLast5 = last5AwayMatchesStats.Wins,
                AwayLossesLast5 = last5AwayMatchesStats.Losses,
                AwayDrawsLast5 = last5AwayMatchesStats.Draws,
                AwayAverageGoalsScoredLast5 = last5AwayMatchesStats.TotalGoalsScored / 5.0, // Avoid integer division
                //AwayAverageGoalsConcededLast5 = last5AwayMatchesStats.TotalGoalsConceded / 5.0 
            }).ToList();

            // Train the model
            var predictorService = new PredictorService();
            var distinctOutcomes = trainingData.Select(md => md.Outcome).Distinct().ToList();
            if (distinctOutcomes.Count >= 2)
            {
                predictorService.TrainModel(trainingData);

                // Predict the outcome for an upcoming match
                var upcomingMatch10 = new MatchData
                {
                    HomeTeam = match.HomeTeam.Name,
                    AwayTeam = match.AwayTeam.Name,
                    HomeScore = last10HomeMatchesStats.TotalGoalsScored / 10,
                    AwayScore = last10AwayMatchesStats.TotalGoalsScored / 10,

                    // Populate new features for upcoming match prediction
                    HomeWinsLast5 = last5HomeMatchesStats.Wins,
                    HomeLossesLast5 = last5HomeMatchesStats.Losses,
                    HomeDrawsLast5 = last5HomeMatchesStats.Draws,
                    HomeAverageGoalsScoredLast5 = last5HomeMatchesStats.TotalGoalsScored / 5.0,
                    //HomeAverageGoalsConcededLast5 = last5HomeMatchesStats.TotalGoalsConceded / 5.0,

                    AwayWinsLast5 = last5AwayMatchesStats.Wins,
                    AwayLossesLast5 = last5AwayMatchesStats.Losses,
                    AwayDrawsLast5 = last5AwayMatchesStats.Draws,
                    AwayAverageGoalsScoredLast5 = last5AwayMatchesStats.TotalGoalsScored / 5.0,
                    //AwayAverageGoalsConcededLast5 = last5AwayMatchesStats.TotalGoalsConceded / 5.0

                };
                var upcomingMatch5 = new MatchData
                {
                    HomeTeam = match.HomeTeam.Name,
                    AwayTeam = match.AwayTeam.Name,
                    HomeScore = last5HomeMatchesStats.TotalGoalsScored / 5,
                    AwayScore = last5AwayMatchesStats.TotalGoalsScored / 5,

                    // Populate new features for upcoming match prediction
                    HomeWinsLast5 = last5HomeMatchesStats.Wins,
                    HomeLossesLast5 = last5HomeMatchesStats.Losses,
                    HomeDrawsLast5 = last5HomeMatchesStats.Draws,
                    HomeAverageGoalsScoredLast5 = last5HomeMatchesStats.TotalGoalsScored / 5.0,
                    //HomeAverageGoalsConcededLast5 = last5HomeMatchesStats.TotalGoalsConceded / 5.0,

                    AwayWinsLast5 = last5AwayMatchesStats.Wins,
                    AwayLossesLast5 = last5AwayMatchesStats.Losses,
                    AwayDrawsLast5 = last5AwayMatchesStats.Draws,
                    AwayAverageGoalsScoredLast5 = last5AwayMatchesStats.TotalGoalsScored / 5.0,
                    //AwayAverageGoalsConcededLast5 = last5AwayMatchesStats.TotalGoalsConceded / 5.0
                };

                var predictedOutcome10 = predictorService.PredictOutcome(upcomingMatch10);
                var predictedOutcome5 = predictorService.PredictOutcome(upcomingMatch10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Predicted Outcome 10 Games for {match.HomeTeam.Name} vs {match.AwayTeam.Name}: {predictedOutcome10}");
                Console.WriteLine($"Predicted Outcome 5 Games for {match.HomeTeam.Name} vs {match.AwayTeam.Name}: {predictedOutcome5}");

                DateTime southAfricaTime = ConvertToSouthAfricaTime(match.UtcDate);

                string output = $"Date: {southAfricaTime:yyyy/MM/dd HH:mm:ss} - Predicted Outcome 10 Games for {match.HomeTeam.Name} vs {match.AwayTeam.Name}: {predictedOutcome10}\n" +
                $"Date: {southAfricaTime:yyyy/MM/dd HH:mm:ss} - Predicted Outcome 5 Games for {match.HomeTeam.Name} vs {match.AwayTeam.Name}: {predictedOutcome5}\n\n";

                string filePath = "C:\\Users\\Godfrey.Masha\\Downloads\\predicted_outcomes1.txt";
                await File.AppendAllTextAsync(filePath, output);
                Console.ResetColor();
            }
        }
    }

    public static DateTime ConvertToSouthAfricaTime(DateTime utcDate)
    {
        // Define the South Africa time zone (UTC+2)
        TimeZoneInfo southAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");

        // Convert UTC to South Africa time
        DateTime southAfricaTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, southAfricaTimeZone);

        return southAfricaTime;
    }
}
