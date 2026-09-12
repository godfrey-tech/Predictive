using ConsoleApp1.Mapper;
using ConsoleApp1.Modals;
using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using CsvHelper;
using CsvHelper.Configuration;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1.Services
{
    public class EPLHistoricalService
    {
        // Historical CSVs live under <repo root>/Data/England Football. Data loading
        // used to hardcode a specific machine's absolute path here (it silently broke
        // every time the repo moved) — walk up from the running assembly instead so
        // it works regardless of which machine or project runs it.
        private static string ResolveDataRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "Data", "England Football");
                if (Directory.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException(
                $"Could not locate 'Data/England Football' by walking up from {AppContext.BaseDirectory}");
        }

        public void HistoricalMatchesFromCSVFiles()
        {
            // Get all CSV files in the specified directory
            string directoryPath = Path.Combine(ResolveDataRoot(), "Premier Leagues");

            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            var countMatches = 0;

            // Process each CSV file one by one
            foreach (var filePath in csvFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");
                Console.WriteLine();

                //var matches = new List<HistoricalMatchFromCSVFile>();

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    if (!csv.Read() || !csv.ReadHeader())
                    {
                        Console.WriteLine("No headers found. Skipping file.");
                        continue;
                    }

                    var headerRecord = csv.Context.Reader.HeaderRecord;
                    if (headerRecord.Count() == 106)
                    {
                        //PremierLeague2019-2020.csv, PremierLeague2020-2021.csv, PremierLeague2021-2022.csv, PremierLeague2022-2023.csv
                        //PremierLeague2023-2024.csv
                        csv.Context.RegisterClassMap<MatchMap>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFile>().ToList();
                        countMatches += matches.Count;
                        // Display the loaded data for the first format
                        foreach (var match in matches)
                        {
                            Console.WriteLine($"{match.Date} - {match.HomeTeam} vs {match.AwayTeam} : {match.FTHG}-{match.FTAG}");
                        }
                    }
                    else if (headerRecord.Count() == 68)
                    {
                        //PremierLeague2013-2014.csv, PremierLeague2014-2015.csv
                        csv.Context.RegisterClassMap<MatchMapFirst>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFirst>().ToList();
                        countMatches += matches.Count;

                        // Display the loaded data for the second format
                        foreach (var match in matches)
                        {
                            Console.WriteLine($"{match.Date} - {match.HomeTeam} vs {match.AwayTeam} : {match.FTHG}-{match.FTAG}");
                        }
                    }
                    else if (headerRecord.Count() == 65)
                    {
                        //PremierLeague2015-2016.csv, PremierLeague2016-2017.csv, PremierLeague2017-2018.csv
                        csv.Context.RegisterClassMap<MatchMapThird>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileThird>().ToList();
                        countMatches += matches.Count;

                        // Display the loaded data for the second format
                        foreach (var match in matches)
                        {
                            Console.WriteLine($"{match.Date} - {match.HomeTeam} vs {match.AwayTeam} : {match.FTHG}-{match.FTAG}");
                        }
                    }
                    else if (headerRecord.Count() == 62)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapFourth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFourth>().ToList();
                        countMatches += matches.Count;

                        // Display the loaded data for the second format
                        foreach (var match in matches)
                        {
                            Console.WriteLine($"{match.Date} - {match.HomeTeam} vs {match.AwayTeam} : {match.FTHG}-{match.FTAG}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized CSV format. Skipping file.");
                    }
                }

            }
            Console.WriteLine($"Total number of premier Leagues historical matches: {countMatches}");
        }

        public List<HistoricalMatchFromCSVFile> LoadAndProcessCompetitionData(string competition)
        {
            // Base directory
            string baseDirectory = ResolveDataRoot();

            // Map competition codes/names to folder paths
            Dictionary<string, string> competitionPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Premier League", "Premier Leagues" },
                { "Championship", "Championship" },
                { "League One", "League One" },
                { "League Two", "League Two" },
                { "Scotland Premier League", @"Scotland\Premier League" },
                { "Scotland Division One", @"Scotland\Division One" },
                { "Bundesliga", @"Germany\Bundesliga One" },
                { "Serie A", @"Italy\Serie A" },
                { "La Liga", @"Spain\La liga" },
                { "League One France", @"France\League One" },
                { "Eredivisie", @"Nehterlands\Eredivisie" },
                { "Jupiler League", @"Belgium\Jupiler League" },
                { "Liga One Portugal", @"Portugal\Liga One" },
                { "Futbol Ligi One", @"Turkey\Futbol Ligi One" },
                // Add more as needed
            };
            if (!competitionPaths.TryGetValue(competition, out string folderPath))
            {
                throw new ArgumentException($"Competition '{competition}' is not recognized.");
            }

            // Full directory path
            string directoryPath = Path.Combine(baseDirectory, folderPath);
            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            var countMatches = 0;
            var _historicalMatchFromCSVFile = new List<HistoricalMatchFromCSVFile>();
            foreach (var filePath in csvFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");
                Console.WriteLine();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null, // Ignore missing fields
                    IgnoreBlankLines = true // Ignore blank lines
                };
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    if (!csv.Read() || !csv.ReadHeader())
                    {
                        Console.WriteLine("No headers found. Skipping file.");
                        continue;
                    }

                    var headerRecord = csv.Context.Reader.HeaderRecord;
                    if (headerRecord.Count() == 120 || headerRecord.Count() == 122 || headerRecord.Count() == 121 || headerRecord.Count() == 131 || headerRecord.Count() == 132)
                    {
                        csv.Context.RegisterClassMap<MatchMapFiveth>();
                        var matsches = csv.GetRecords<HistoricalMatchFromCSVFileFiveth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV5(matsches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 106)
                    {
                        csv.Context.RegisterClassMap<MatchMap>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFile>().ToList();
                        _historicalMatchFromCSVFile.AddRange(matches);
                        //break;
                    }
                    else if (headerRecord.Count() == 68)
                    {
                        //PremierLeague2013-2014.csv, PremierLeague2014-2015.csv
                        csv.Context.RegisterClassMap<MatchMapFirst>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFirst>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV1(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 65)
                    {
                        //PremierLeague2015-2016.csv, PremierLeague2016-2017.csv, PremierLeague2017-2018.csv
                        csv.Context.RegisterClassMap<MatchMapThird>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileThird>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV3(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 62)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapFourth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFourth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV4(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 55)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapSixth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileSixth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV6(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 52)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapSeventh>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileSeventh>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV7(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 67 || headerRecord.Count() == 64 || headerRecord.Count() == 61)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapEighth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileEighth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV8(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 105)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapNinth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileEighth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV8(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 119)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapTenth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileNinth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV9(matches);
                        //_historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 22 || headerRecord.Count() == 25)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapExtraLeague>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileExtraLeague>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileExtraLeague(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized CSV format. Skipping file.");
                    }
                }
            }
            return _historicalMatchFromCSVFile;

        }
        public List<HistoricalMatchFromCSVFile> LoadAndProcessChampionShipData()
        {

            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Premier Leagues";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Championship";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\League One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\League Two";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Scotland\Premier League";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Scotland\Division One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Scotland\Division Two";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Germany\Bundesliga One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Italy\Serie A";
            string directoryPath = Path.Combine(ResolveDataRoot(), "Spain", "La liga");
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\France\League One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Nehterlands\Eredivisie";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Belgium\Jupiler League";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Portugal\Liga One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Turkey\Futbol Ligi One";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Greece\Ethniki Katigoria";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Argentina";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Brasil";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\USA";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\DenMark";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Sweden";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Romania";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Switzerland";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Austria";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Poland";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Japan";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Norway";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Ireland";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\Finland";
            //string directoryPath = @"C:\Users\Godfrey.Masha\source\repos\Predictvie\Data\England Football\Extra Leagues\China";

            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            var countMatches = 0;
            var _historicalMatchFromCSVFile = new List<HistoricalMatchFromCSVFile>();
            foreach (var filePath in csvFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");
                Console.WriteLine();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null, // Ignore missing fields
                    IgnoreBlankLines = true // Ignore blank lines
                };
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    if (!csv.Read() || !csv.ReadHeader())
                    {
                        Console.WriteLine("No headers found. Skipping file.");
                        continue;
                    }

                    var headerRecord = csv.Context.Reader.HeaderRecord;
                    if (headerRecord.Count() == 120 || headerRecord.Count() == 122 || headerRecord.Count() == 121 || headerRecord.Count() == 131 || headerRecord.Count() == 132)
                    {
                        csv.Context.RegisterClassMap<MatchMapFiveth>();
                        var matsches = csv.GetRecords<HistoricalMatchFromCSVFileFiveth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV5(matsches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 106)
                    {
                        csv.Context.RegisterClassMap<MatchMap>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFile>().ToList();
                        _historicalMatchFromCSVFile.AddRange(matches);
                        //break;
                    }
                    else if (headerRecord.Count() == 68)
                    {
                        //PremierLeague2013-2014.csv, PremierLeague2014-2015.csv
                        csv.Context.RegisterClassMap<MatchMapFirst>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFirst>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV1(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 65)
                    {
                        //PremierLeague2015-2016.csv, PremierLeague2016-2017.csv, PremierLeague2017-2018.csv
                        csv.Context.RegisterClassMap<MatchMapThird>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileThird>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV3(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 62)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapFourth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFourth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV4(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 55)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapSixth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileSixth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV6(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 52)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapSeventh>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileSeventh>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV7(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 67 || headerRecord.Count() == 64 || headerRecord.Count() == 61)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapEighth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileEighth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV8(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 105)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapNinth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileEighth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV8(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 119)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapTenth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileNinth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV9(matches);
                        //_historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 22 || headerRecord.Count() == 25)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapExtraLeague>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileExtraLeague>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileExtraLeague(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized CSV format. Skipping file.");
                    }
                }
            }
            return _historicalMatchFromCSVFile;
        }
        public  List<HistoricalMatchFromCSVFile> LoadAndProcessData()
        {
            string directoryPath = Path.Combine(ResolveDataRoot(), "Premier Leagues");
            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            var countMatches = 0;

            // Process each CSV file one by one
            var _historicalMatchFromCSVFile = new List<HistoricalMatchFromCSVFile>();
            foreach (var filePath in csvFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");
                Console.WriteLine();

                //var matches = new List<HistoricalMatchFromCSVFile>();

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    if (!csv.Read() || !csv.ReadHeader())
                    {
                        Console.WriteLine("No headers found. Skipping file.");
                        continue;
                    }

                    var headerRecord = csv.Context.Reader.HeaderRecord;
                    if (headerRecord.Count() == 120)
                    {
                        csv.Context.RegisterClassMap<MatchMapFiveth>();
                        var matsches = csv.GetRecords<HistoricalMatchFromCSVFileFiveth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV5(matsches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 106)
                    {
                        csv.Context.RegisterClassMap<MatchMap>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFile>().ToList();
                        _historicalMatchFromCSVFile.AddRange(matches);
                        //break;
                    }
                    else if (headerRecord.Count() == 68)
                    {
                        //PremierLeague2013-2014.csv, PremierLeague2014-2015.csv
                        csv.Context.RegisterClassMap<MatchMapFirst>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFirst>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV1(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 65)
                    {
                        //PremierLeague2015-2016.csv, PremierLeague2016-2017.csv, PremierLeague2017-2018.csv
                        csv.Context.RegisterClassMap<MatchMapThird>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileThird>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV3(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else if (headerRecord.Count() == 62)
                    {
                        //PremierLeague2018-2019.csv
                        csv.Context.RegisterClassMap<MatchMapFourth>();
                        var matches = csv.GetRecords<HistoricalMatchFromCSVFileFourth>().ToList();
                        var seasonMatches = MatchMapper.MapToHistoricalMatchFromCSVFileV4(matches);
                        _historicalMatchFromCSVFile.AddRange(seasonMatches);
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized CSV format. Skipping file.");
                    }
                }
            }
            return _historicalMatchFromCSVFile;
        }

        public List<MatchDataV2> GetTeamStats(List<MatchDataV2> allHistoricalMatchData, string homeTeam, string awayTeam)
        {
            var filteredData = allHistoricalMatchData.Where(m =>
                (m.HostHomeTeam.ToLower() == homeTeam.ToLower() && m.AwayTeam.ToLower() == awayTeam.ToLower()))
                .ToList();

            return filteredData;
        }

        public void FileOutputCsvV3(List<MatchDataV2> data)
        {
            string filePath = "C:\\Users\\Godfrey.Masha\\Downloads\\modalOutput.csv";

            // Write the header line
            string header = "DateOne,HostHomeTeam,AwayTeam,HostHomeGoalsAvg,HostHomeGoalsConcededAvg," +
                            "HostHomeWinsLast5,HostHomeLossesLast5,HostHomeDrawsLast5,HostHomeAverageGoalsScoredLast5," +
                            "HostAwayGoalsAvg,HostAwayGoalsConcededAvg,HostAwayWinsLast5,HostAwayLossesLast5," +
                            "HostAwayDrawsLast5,HostAwayAverageGoalsScoredLast5," +
                            "OpponentHomeGoalsAvg,OpponentHomeGoalsConcededAvg,OpponentHomeWinsLast5," +
                            "OpponentHomeLossesLast5,OpponentHomeDrawsLast5,OpponentHomeAverageGoalsScoredLast5," +
                            "OpponentAwayGoalsAvg,OpponentAwayGoalsConcededAvg,OpponentAwayWinsLast5," +
                            "OpponentAwayLossesLast5,OpponentAwayDrawsLast5,OpponentAwayAverageGoalsScoredLast5," +
                            "HeadToHeadWinsHome,HeadToHeadWinsAway,HeadToHeadDraws,Outcome\n";

            // Write the header to the CSV file
            File.WriteAllText(filePath, header);

            foreach (var item in data)
            {
                // Create a CSV line for each item, ensuring float values use "." as the decimal separator
                string output = $"{item.DateOne.ToString("yyyy-MM-dd")},{item.HostHomeTeam},{item.AwayTeam}," +
                                $"{item.HostHomeGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HostHomeGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HostHomeWinsLast5},{item.HostHomeLossesLast5},{item.HostHomeDrawsLast5}," +
                                $"{item.HostHomeAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HostAwayGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HostAwayGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HostAwayWinsLast5},{item.HostAwayLossesLast5},{item.HostAwayDrawsLast5}," +
                                $"{item.HostAwayAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentHomeGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentHomeGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentHomeWinsLast5},{item.OpponentHomeLossesLast5},{item.OpponentHomeDrawsLast5}," +
                                $"{item.OpponentHomeAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentAwayGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentAwayGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.OpponentAwayWinsLast5},{item.OpponentAwayLossesLast5},{item.OpponentAwayDrawsLast5}," +
                                $"{item.OpponentAwayAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HeadToHeadWinsHome},{item.HeadToHeadWinsAway},{item.HeadToHeadDraws},{item.Outcome}\n";

                // Append the CSV line to the file
                File.AppendAllText(filePath, output);
            }
        }

        public List<MatchDataV2> PrepareMatchDataV2(List<HistoricalMatchFromCSVFile> matches)
        {
            var matchDataList = new List<MatchDataV2>();

            // Group matches by team
            var teams = matches.Select(m => m.HomeTeam).Union(matches.Select(m => m.AwayTeam)).Distinct().ToList();

            foreach (var team in teams)
            {
                // Get all matches for the team (both home and away)
                var allMatches = matches.Where(m => m.HomeTeam == team || m.AwayTeam == team).ToList();

                // Get home matches for the team
                var homeMatches = allMatches.Where(m => m.HomeTeam == team).ToList();
                // Get away matches for the team
                var awayMatches = allMatches.Where(m => m.AwayTeam == team).ToList();

                // Calculate averages for home matches
                float homeGoalsAvg = homeMatches.Any() ? (float)homeMatches.Average(m => m.FTHG ?? 0) : 0;
                float homeGoalsConcededAvg = homeMatches.Any() ? (float)homeMatches.Average(m => m.FTAG ?? 0) : 0;

                // Calculate averages for away matches
                float awayGoalsAvg = awayMatches.Any() ? (float)awayMatches.Average(m => m.FTAG ?? 0) : 0;
                float awayGoalsConcededAvg = awayMatches.Any() ? (float)awayMatches.Average(m => m.FTHG ?? 0) : 0;

                // Recent form metrics for the last 5 matches (home or away)
                int hostWinsLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.HomeTeam == team && m.FTR == "H") || (m.AwayTeam == team && m.FTR == "A"));
                int hostLossesLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.HomeTeam == team && m.FTR == "A") || (m.AwayTeam == team && m.FTR == "H"));
                int hostDrawsLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float hostAverageGoalsScoredLast5 = (float)allMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.HomeTeam == team ? m.FTHG ?? 0 : m.FTAG ?? 0);
                float hostAverageGoalsConcededLast5 = (float)allMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.HomeTeam == team ? m.FTAG ?? 0 : m.FTHG ?? 0);

                // Calculate recent form metrics for the away team
                int awayWinsLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.AwayTeam == team && m.FTR == "A"));
                int awayLossesLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.AwayTeam == team && m.FTR == "H") || (m.HomeTeam == team && m.FTR == "A"));
                int awayDrawsLast5 = allMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float awayAverageGoalsScoredLast5 = (float)allMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.AwayTeam == team ? m.FTAG ?? 0 : m.FTHG ?? 0);

                // Calculate head-to-head statistics
                foreach (var opponent in teams)
                {
                    if (team != opponent) // Avoid self-matching
                    {
                        int headToHeadWinsHome = matches.Count(m => m.HomeTeam == team && m.AwayTeam == opponent && m.FTR == "H");
                        int headToHeadWinsAway = matches.Count(m => m.AwayTeam == team && m.HomeTeam == opponent && m.FTR == "A");
                        int headToHeadDraws = matches.Count(m => (m.HomeTeam == team && m.AwayTeam == opponent && m.FTR == "D") ||
                                                                  (m.AwayTeam == team && m.HomeTeam == opponent && m.FTR == "D"));
                        // Calculate opponent stats (last 5 matches)
                        var opponentMatches = matches.Where(m => m.HomeTeam == opponent || m.AwayTeam == opponent).ToList();

                        // Calculate averages for the opponent in their last 5 matches
                        float opponentGoalsAvgLast5 = (float)opponentMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.HomeTeam == opponent ? m.FTHG ?? 0 : m.FTAG ?? 0);
                        float opponentGoalsConcededAvgLast5 = (float)opponentMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.HomeTeam == opponent ? m.FTAG ?? 0 : m.FTHG ?? 0);
                        int opponentWinsLast5 = opponentMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.HomeTeam == opponent && m.FTR == "H") || (m.AwayTeam == opponent && m.FTR == "A"));
                        int opponentLossesLast5 = opponentMatches.OrderByDescending(m => m.Date).Take(5).Count(m => (m.HomeTeam == opponent && m.FTR == "A") || (m.AwayTeam == opponent && m.FTR == "H"));
                        int opponentDrawsLast5 = opponentMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                        float opponentAverageGoalsScoredLast5 = (float)opponentMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.HomeTeam == opponent ? m.FTHG ?? 0 : m.FTAG ?? 0);

                        // Get matches where the opponent is playing at home
                        var opponentHomeMatches = matches.Where(m => m.HomeTeam == opponent)
                                                         .OrderByDescending(m => m.Date)
                                                         .Take(5)
                                                         .ToList();

                        // Calculate averages for the opponent in their last 5 home matches
                        float opponentHomeGoalsAvgLast5 = (float)opponentHomeMatches.Average(m => m.FTHG ?? 0); // Average goals scored by the opponent when playing at home
                        float opponentHomeGoalsConcededAvgLast5 = (float)opponentHomeMatches.Average(m => m.FTAG ?? 0); // Average goals conceded by the opponent when playing at home
                        int opponentHomeWinsLast5 = opponentHomeMatches.Count(m => m.FTR == "H"); // Count of wins when playing at home
                        int opponentHomeLossesLast5 = opponentHomeMatches.Count(m => m.FTR == "A"); // Count of losses when playing at home
                        int opponentHomeDrawsLast5 = opponentHomeMatches.Count(m => m.FTR == "D"); // Count of draws when playing at home

                        // Get matches where the opponent is playing away
                        var opponentAwayMatches = matches.Where(m => m.AwayTeam == opponent)
                                                         .OrderByDescending(m => m.Date)
                                                         .Take(5)
                                                         .ToList();

                        // Calculate averages for the opponent in their last 5 away matches
                        float opponentAwayGoalsAvgLast5 = (float)opponentAwayMatches.Average(m => m.FTAG ?? 0); // Average goals scored by the opponent when playing away
                        float opponentAwayGoalsConcededAvgLast5 = (float)opponentAwayMatches.Average(m => m.FTHG ?? 0); // Average goals conceded by the opponent when playing away
                        int opponentAwayWinsLast5 = opponentAwayMatches.Count(m => m.FTR == "A"); // Count of wins when playing away
                        int opponentAwayLossesLast5 = opponentAwayMatches.Count(m => m.FTR == "H"); // Count of losses when playing away
                        int opponentAwayDrawsLast5 = opponentAwayMatches.Count(m => m.FTR == "D"); // Count of draws when playing away

                        // Add the matches to the match data list
                        foreach (var match in homeMatches)
                        {
                            matchDataList.Add(new MatchDataV2
                            {
                                DateOne = match.Date,
                                HostHomeTeam = team,
                                AwayTeam = opponent,
                                HostHomeGoalsAvg = homeGoalsAvg,
                                HostHomeGoalsConcededAvg = homeGoalsConcededAvg,
                                HostHomeWinsLast5 = hostWinsLast5,
                                HostHomeLossesLast5 = hostLossesLast5,
                                HostHomeDrawsLast5 = hostDrawsLast5,
                                HostHomeAverageGoalsScoredLast5 = hostAverageGoalsScoredLast5,
                                HostAwayGoalsAvg = awayGoalsAvg, // Stats for the away team
                                HostAwayGoalsConcededAvg = awayGoalsConcededAvg, // Stats for the away team
                                HostAwayWinsLast5 = awayWinsLast5, // Stats for the away team
                                HostAwayLossesLast5 = awayLossesLast5, // Stats for the away team
                                HostAwayDrawsLast5 = awayDrawsLast5, // Stats for the away team
                                HostAwayAverageGoalsScoredLast5 = awayAverageGoalsScoredLast5, // Stats for the away team

                                HostAverageGoalsScoredLast5 = hostAverageGoalsConcededLast5,
                                HostDrawsLast5 = hostDrawsLast5,
                                HostLossesLast5 = hostLossesLast5,
                                HostWinsLast5 = hostWinsLast5,

                                OpponentAverageGoalsScoredLast5 = opponentAverageGoalsScoredLast5,
                                OpponentDrawsLast5 = opponentDrawsLast5,
                                OpponentLossesLast5 = opponentLossesLast5,
                                OpponentWinsLast5 = opponentWinsLast5,

                                OpponentHomeGoalsAvg = opponentHomeGoalsAvgLast5, // Stats for the opponent at home
                                OpponentHomeGoalsConcededAvg = opponentHomeGoalsConcededAvgLast5, // Stats for the opponent at home
                                OpponentHomeWinsLast5 = opponentHomeWinsLast5, // Stats for the opponent at home
                                OpponentHomeLossesLast5 = opponentHomeLossesLast5, // Stats for the opponent at home
                                OpponentHomeDrawsLast5 = opponentHomeDrawsLast5, // Stats for the opponent at home
                                OpponentAwayGoalsAvg = opponentAwayGoalsAvgLast5, // Stats for the opponent away
                                OpponentAwayGoalsConcededAvg = opponentAwayGoalsConcededAvgLast5, // Stats for the opponent away
                                OpponentAwayWinsLast5 = opponentAwayWinsLast5, // Stats for the opponent away
                                OpponentAwayLossesLast5 = opponentAwayLossesLast5, // Stats for the opponent away
                                OpponentAwayDrawsLast5 = opponentAwayDrawsLast5, // Stats for the opponent away

                                HeadToHeadWinsHome = headToHeadWinsHome,
                                HeadToHeadWinsAway = headToHeadWinsAway,
                                HeadToHeadDraws = headToHeadDraws,
                                Outcome = match.FTR switch
                                {
                                    "H" => "HomeWin",
                                    "D" => "Draw",
                                    "A" => "AwayWin",
                                    _ => "Unknown"
                                }
                            });
                        }
                    }
                }
            }

            return matchDataList; // Don't forget to return the populated list
        }

        public List<MatchDataV1> PrepareMatchData(List<HistoricalMatchFromCSVFile> matches)
        {
            var matchDataList = new List<MatchDataV1>();

            // Group matches by team
            var teams = matches.Select(m => m.HomeTeam).Union(matches.Select(m => m.AwayTeam)).Distinct().ToList();

            foreach (var team in teams)
            {
                // Get home matches for the team
                var homeMatches = matches.Where(m => m.HomeTeam == team).ToList();
                // Get away matches for the team
                var awayMatches = matches.Where(m => m.AwayTeam == team).ToList();

                // Calculate averages for home matches
                float homeGoalsAvg = homeMatches.Any() ? (float)homeMatches.Average(m => m.FTHG ?? 0) : 0;
                float homeGoalsConcededAvg = homeMatches.Any() ? (float)homeMatches.Average(m => m.FTAG ?? 0) : 0;

                // Calculate averages for away matches
                float awayGoalsAvg = awayMatches.Any() ? (float)awayMatches.Average(m => m.FTAG ?? 0) : 0;
                float awayGoalsConcededAvg = awayMatches.Any() ? (float)awayMatches.Average(m => m.FTHG ?? 0) : 0;

                // Recent form metrics for home matches
                int homeWinsLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "H");
                int homeLossesLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "A");
                int homeDrawsLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float homeAverageGoalsScoredLast5 = (float)homeMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.FTHG ?? 0);

                // Recent form metrics for away matches
                int awayWinsLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "A");
                int awayLossesLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "H");
                int awayDrawsLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float awayAverageGoalsScoredLast5 = (float)awayMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.FTAG ?? 0);

                // Calculate head-to-head statistics
                foreach (var opponent in teams)
                {
                    if (team != opponent) // Avoid self-matching
                    {
                        int headToHeadWinsHome = matches.Count(m => m.HomeTeam == team && m.AwayTeam == opponent && m.FTR == "H");
                        int headToHeadWinsAway = matches.Count(m => m.AwayTeam == team && m.HomeTeam == opponent && m.FTR == "A");
                        int headToHeadDraws = matches.Count(m => (m.HomeTeam == team && m.AwayTeam == opponent && m.FTR == "D") ||
                                                                 (m.AwayTeam == team && m.HomeTeam == opponent && m.FTR == "D"));

                        // Add the home matches to the match data list
                        foreach (var match in homeMatches)
                        {
                            matchDataList.Add(new MatchDataV1
                            {
                                DateOne = match.Date,
                                HomeTeam = team,
                                AwayTeam = opponent,
                                HomeGoalsAvg = homeGoalsAvg,
                                AwayGoalsAvg = awayGoalsAvg,
                                HomeGoalsConcededAvg = homeGoalsConcededAvg,
                                AwayGoalsConcededAvg = awayGoalsConcededAvg,
                                HomeWinsLast5 = homeWinsLast5,
                                HomeLossesLast5 = homeLossesLast5,
                                HomeDrawsLast5 = homeDrawsLast5,
                                HomeAverageGoalsScoredLast5 = homeAverageGoalsScoredLast5,
                                AwayWinsLast5 = awayWinsLast5,
                                AwayLossesLast5 = awayLossesLast5,
                                AwayDrawsLast5 = awayDrawsLast5,
                                AwayAverageGoalsScoredLast5 = awayAverageGoalsScoredLast5,
                                HeadToHeadWinsHome = headToHeadWinsHome,
                                HeadToHeadWinsAway = headToHeadWinsAway,
                                HeadToHeadDraws = headToHeadDraws,
                                Outcome = match.FTR switch
                                {
                                    "H" => "HomeWin",
                                    "D" => "Draw",
                                    "A" => "AwayWin",
                                    _ => "Unknown"
                                }
                            });
                        }

                        // Add the away matches to the match data list
                        foreach (var match in awayMatches)
                        {
                            matchDataList.Add(new MatchDataV1
                            {
                                //Date = SetDate(match.Date),
                                DateOne = match.Date,
                                HomeTeam = match.HomeTeam,
                                AwayTeam = team,
                                HomeGoalsAvg = homeGoalsAvg,
                                AwayGoalsAvg = awayGoalsAvg,
                                HomeGoalsConcededAvg = awayGoalsConcededAvg,
                                AwayGoalsConcededAvg = awayGoalsConcededAvg,
                                HomeWinsLast5 = homeWinsLast5,
                                HomeLossesLast5 = homeLossesLast5,
                                HomeDrawsLast5 = homeDrawsLast5,
                                HomeAverageGoalsScoredLast5 = homeAverageGoalsScoredLast5,
                                AwayWinsLast5 = awayWinsLast5,
                                AwayLossesLast5 = awayLossesLast5,
                                AwayDrawsLast5 = awayDrawsLast5,
                                AwayAverageGoalsScoredLast5 = awayAverageGoalsScoredLast5,
                                HeadToHeadWinsHome = headToHeadWinsHome,
                                HeadToHeadWinsAway = headToHeadWinsAway,
                                HeadToHeadDraws = headToHeadDraws,
                                Outcome = match.FTR switch
                                {
                                    "H" => "AwayWin",
                                    "D" => "Draw",
                                    "A" => "HomeWin",
                                    _ => "Unknown"
                                }
                            });
                        }
                    }
                }
            }
            //FileOutputCsv(matchDataList);
            return matchDataList;
        }

        public void FileOutputCsv(List<MatchDataV1> data)
        {
            string filePath = "C:\\Users\\Godfrey.Masha\\Downloads\\modalOutput.csv";

            // Write the header line
            string header = "Date,HomeTeam,AwayTeam,HomeGoalsAvg,AwayGoalsAvg,HomeGoalsConcededAvg,AwayGoalsConcededAvg," +
                            "HomeWinsLast5,HomeLossesLast5,HomeDrawsLast5,HomeAverageGoalsScoredLast5," +
                            "AwayWinsLast5,AwayLossesLast5,AwayDrawsLast5,AwayAverageGoalsScoredLast5," +
                            "HST,AST,HeadToHeadWinsHome,HeadToHeadWinsAway,HeadToHeadDraws,Outcome\n";

            // Write the header to the CSV file
            File.WriteAllText(filePath, header);

            foreach (var item in data)
            {
                // Create a CSV line for each item, ensuring float values use "." as the decimal separator
                string output = $"{item.DateOne},{item.HomeTeam},{item.AwayTeam}," +
                                $"{item.HomeGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.AwayGoalsAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HomeGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.AwayGoalsConcededAvg.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HomeWinsLast5},{item.HomeLossesLast5},{item.HomeDrawsLast5}," +
                                $"{item.HomeAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.AwayWinsLast5}," +
                                $"{item.AwayLossesLast5},{item.AwayDrawsLast5}," +
                                $"{item.AwayAverageGoalsScoredLast5.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}," +
                                $"{item.HST},{item.AST}," +
                                $"{item.HeadToHeadWinsHome},{item.HeadToHeadWinsAway}," +
                                $"{item.HeadToHeadDraws},{item.Outcome}\n";

                // Append the CSV line to the file
                File.AppendAllText(filePath, output);
            }
        }
        public long SetDate(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }
        public List<MatchDataV1> PrepareMatchDataVOld(List<HistoricalMatchFromCSVFile> matches)
        {
            var matchDataList = new List<MatchDataV1>();

            // Group matches by team
            var teams = matches.Select(m => m.HomeTeam).Union(matches.Select(m => m.AwayTeam)).Distinct().ToList();

            foreach (var team in teams)
            {
                // Get home matches for the team
                var homeMatches = matches.Where(m => m.HomeTeam == team).ToList();
                // Get away matches for the team
                var awayMatches = matches.Where(m => m.AwayTeam == team).ToList();

                // Calculate averages for home matches
                float homeGoalsAvg = (float)homeMatches.Average(m => m.FTHG ?? 0);
                float homeGoalsConcededAvg = (float)homeMatches.Average(m => m.FTAG ?? 0);

                // Calculate averages for away matches
                float awayGoalsAvg = (float)awayMatches.Average(m => m.FTAG ?? 0);
                float awayGoalsConcededAvg = (float)awayMatches.Average(m => m.FTHG ?? 0);

                // Recent form metrics for home matches
                int homeWinsLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "H");
                int homeLossesLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "A");
                int homeDrawsLast5 = homeMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float homeAverageGoalsScoredLast5 = (float)homeMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.FTHG ?? 0);

                // Recent form metrics for away matches
                int awayWinsLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "A");
                int awayLossesLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "H");
                int awayDrawsLast5 = awayMatches.OrderByDescending(m => m.Date).Take(5).Count(m => m.FTR == "D");
                float awayAverageGoalsScoredLast5 = (float)awayMatches.OrderByDescending(m => m.Date).Take(5).Average(m => m.FTAG ?? 0);

                // Calculate shots on target averages
                int totalHST = homeMatches.Sum(m => m.HST ?? 0);
                int totalAST = awayMatches.Sum(m => m.AST ?? 0);
                int homeMatchesCount = homeMatches.Count;
                int awayMatchesCount = awayMatches.Count;
                // Average shots on target
                float homeShotsOnTargetAvg = homeMatchesCount > 0 ? (float)totalHST / homeMatchesCount : 0;
                float awayShotsOnTargetAvg = awayMatchesCount > 0 ? (float)totalAST / awayMatchesCount : 0;

                // Calculate head-to-head statistics
                //int headToHeadWinsHome = matches.Count(m => m.HomeTeam == team && m.AwayTeam == match.AwayTeam && m.FTR == "H");
                //int headToHeadWinsAway = matches.Count(m => m.AwayTeam == team && m.HomeTeam == match.HomeTeam && m.FTR == "A");
                //int headToHeadDraws = matches.Count(m => (m.HomeTeam == team && m.AwayTeam == match.AwayTeam && m.FTR == "D") ||
                //                                         (m.AwayTeam == team && m.HomeTeam == match.HomeTeam && m.FTR == "D"));

                // Calculate outcomes for home matches
                foreach (var match in homeMatches)
                {
                    string outcome = match.FTR switch
                    {
                        "H" => "HomeWin",
                        "D" => "Draw",
                        "A" => "AwayWin",
                        _ => "Unknown"
                    };

                    matchDataList.Add(new MatchDataV1
                    {
                        HomeTeam = team,
                        AwayTeam = match.AwayTeam,
                        HomeGoalsAvg = homeGoalsAvg,
                        AwayGoalsAvg = awayGoalsAvg,
                        HomeGoalsConcededAvg = homeGoalsConcededAvg,
                        AwayGoalsConcededAvg = awayGoalsConcededAvg,
                        HomeWinsLast5 = homeWinsLast5,
                        HomeLossesLast5 = homeLossesLast5,
                        HomeDrawsLast5 = homeDrawsLast5,
                        HomeAverageGoalsScoredLast5 = homeAverageGoalsScoredLast5,
                        AwayWinsLast5 = awayWinsLast5,
                        AwayLossesLast5 = awayLossesLast5,
                        AwayDrawsLast5 = awayDrawsLast5,
                        AwayAverageGoalsScoredLast5 = awayAverageGoalsScoredLast5,
                        HST = homeShotsOnTargetAvg, // Set the average shots on target
                        AST = awayShotsOnTargetAvg, // Set the average shots on target

                        Outcome = outcome // Set the outcome based on FTR
                    });
                }

                // Calculate outcomes for away matches
                foreach (var match in awayMatches)
                {
                    string outcome = match.FTR switch
                    {
                        "H" => "AwayWin",
                        "D" => "Draw",
                        "A" => "HomeWin",
                        _ => "Unknown"
                    };

                    matchDataList.Add(new MatchDataV1
                    {
                        HomeTeam = match.HomeTeam,
                        AwayTeam = team,
                        HomeGoalsAvg = awayGoalsAvg,
                        AwayGoalsAvg = awayGoalsAvg,
                        HomeGoalsConcededAvg = awayGoalsConcededAvg,
                        AwayGoalsConcededAvg = awayGoalsConcededAvg,
                        HomeWinsLast5 = homeWinsLast5,
                        HomeLossesLast5 = homeLossesLast5,
                        HomeDrawsLast5 = homeDrawsLast5,
                        HomeAverageGoalsScoredLast5 = homeAverageGoalsScoredLast5,
                        AwayWinsLast5 = awayWinsLast5,
                        AwayLossesLast5 = awayLossesLast5,
                        AwayDrawsLast5 = awayDrawsLast5,
                        AwayAverageGoalsScoredLast5 = awayAverageGoalsScoredLast5,
                        HST = homeShotsOnTargetAvg, // Set the average shots on target
                        AST = awayShotsOnTargetAvg, // Set the average shots on target

                        Outcome = outcome // Set the outcome based on FTR
                    });
                }
            }

            return matchDataList;
        }

    }
}
