using ConsoleApp1.Modals;
using ConsoleApp1.Services;
using MathNet.Numerics.Statistics;
using Microsoft.ML;
using Microsoft.ML.Data;
using Polly;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PredictorService
{
    private MLContext mlContext;
    private ITransformer model;

    public PredictorService()
    {
        mlContext = new MLContext();
    }
    //private MLContext mlContext = new MLContext();
    //private ITransformer model;

    public async Task<string> PredictMatchOutcome(string homeTeam, string awayTeam, int homeTeamHistoricalData, int awayTeamHistoricalData)
    {
        try
        {
            // Fetch historical data for both teams
            var dataFetcher = new DataFetcher();

            //var homeTeamHistoricalData = await dataFetcher.GetHistoricalDataForFixture(homeTeam, awayTeam, homeTeamId);
            //var awayTeamHistoricalData = await dataFetcher.GetHistoricalDataForFixture(awayTeam, homeTeam, awayTeamId);

            // Create a MatchData object for the prediction
            var upcomingMatch = new MatchData
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeScore = homeTeamHistoricalData, // Example calculation
                AwayScore = awayTeamHistoricalData // Example calculation
            };

            // Use the trained model to predict the outcome
            var predictedOutcome = PredictOutcome(upcomingMatch);
            return predictedOutcome;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void TrainModel(List<MatchData> trainingData)
    {
        try
        {
            // Ensure that training data is not null or empty
            if (trainingData == null || !trainingData.Any())
            {
                throw new ArgumentException("Training data cannot be null or empty.", nameof(trainingData));
            }

            // Check for distinct outcomes
            var distinctOutcomes = trainingData.Select(md => md.Outcome).Distinct().ToList();
            if (distinctOutcomes.Count < 2)
            {
                throw new ArgumentException("Training data must contain at least two distinct outcomes.", nameof(trainingData));
            }

            var trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);

            //var pipeline = mlContext.Transforms.Concatenate("Features", "HomeScore", "AwayScore")
            //    .Append(mlContext.Transforms.Conversion.MapValueToKey("Outcome")) // Convert Outcome to Key
            //    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Outcome", maximumNumberOfIterations: 100));

            var pipeline = mlContext.Transforms.Concatenate("Features",
                "HomeScore",
                "AwayScore",
                "HomeWinsLast5",
                "HomeLossesLast5",
                "HomeDrawsLast5",
                "HomeAverageGoalsScoredLast5",
                //"HomeAverageGoalsConcededLast5",
                "AwayWinsLast5",
                "AwayLossesLast5",
                "AwayDrawsLast5",
                "AwayAverageGoalsScoredLast5"
                //"AwayAverageGoalsConcededLast5"
                )
    .Append(mlContext.Transforms.Conversion.MapValueToKey("Outcome")) // Convert Outcome to Key
    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Outcome", maximumNumberOfIterations: 100));


            model = pipeline.Fit(trainingDataView); // Set the model variable
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while training the model: {ex.Message}");
            throw; // Re-throw the exception after logging
        }
    }
    public List<MatchDataV1> LoadAndPrepareData()
    {
        return new List<MatchDataV1>
        {
            new MatchDataV1 { HomeTeam = "Liverpool", AwayTeam = "Chelsea", HomeGoalsAvg = 2.5f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.2f, AwayGoalsConcededAvg = 1.5f, Outcome = "HomeWin" },
            new MatchDataV1 { HomeTeam = "Manchester City", AwayTeam = "Arsenal", HomeGoalsAvg = 3.0f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 0.8f, AwayGoalsConcededAvg = 2.0f, Outcome = "HomeWin" },
            new MatchDataV1 { HomeTeam = "Tottenham", AwayTeam = "Manchester United", HomeGoalsAvg = 1.5f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.0f, AwayGoalsConcededAvg = 1.0f, Outcome = "Draw" },
            new MatchDataV1 { HomeTeam = "Arsenal", AwayTeam = "Everton", HomeGoalsAvg = 2.2f, AwayGoalsAvg = 1.0f, HomeGoalsConcededAvg = 1.1f, AwayGoalsConcededAvg = 1.8f, Outcome = "HomeWin" },
            new MatchDataV1 { HomeTeam = "Liverpool", AwayTeam = "West Ham", HomeGoalsAvg = 2.8f, AwayGoalsAvg = 1.2f, HomeGoalsConcededAvg = 1.0f, AwayGoalsConcededAvg = 1.5f, Outcome = "HomeWin" },
            new MatchDataV1 { HomeTeam = "Manchester United", AwayTeam = "Leicester", HomeGoalsAvg = 1.9f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.5f, AwayGoalsConcededAvg = 1.2f, Outcome = "AwayWin" },
            new MatchDataV1 { HomeTeam = "Manchester United", AwayTeam = "Leicester", HomeGoalsAvg = 1.9f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.5f, AwayGoalsConcededAvg = 1.2f, Outcome = "AwayWin" },
            new MatchDataV1 { HomeTeam = "Manchester United", AwayTeam = "Leicester", HomeGoalsAvg = 1.9f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.5f, AwayGoalsConcededAvg = 1.2f, Outcome = "AwayWin" },
            new MatchDataV1 { HomeTeam = "Manchester United", AwayTeam = "Leicester", HomeGoalsAvg = 1.9f, AwayGoalsAvg = 1.5f, HomeGoalsConcededAvg = 1.5f, AwayGoalsConcededAvg = 1.2f, Outcome = "AwayWin" },
            new MatchDataV1 { HomeTeam = "Chelsea", AwayTeam = "Southampton", HomeGoalsAvg = 2.0f, AwayGoalsAvg = 1.3f, HomeGoalsConcededAvg = 1.0f, AwayGoalsConcededAvg = 1.4f, Outcome = "Draw" },
            new MatchDataV1 { HomeTeam = "Aston Villa", AwayTeam = "Newcastle", HomeGoalsAvg = 1.5f, AwayGoalsAvg = 2.0f, HomeGoalsConcededAvg = 1.2f, AwayGoalsConcededAvg = 1.8f, Outcome = "AwayWin" },
            new MatchDataV1 { HomeTeam = "Brighton", AwayTeam = "Crystal Palace", HomeGoalsAvg = 1.3f, AwayGoalsAvg = 1.4f, HomeGoalsConcededAvg = 1.1f, AwayGoalsConcededAvg = 1.2f, Outcome = "Draw" },
            new MatchDataV1 { HomeTeam = "Brighton", AwayTeam = "Crystal Palace", HomeGoalsAvg = 1.3f, AwayGoalsAvg = 1.4f, HomeGoalsConcededAvg = 1.1f, AwayGoalsConcededAvg = 1.2f, Outcome = "Draw" },
            new MatchDataV1 { HomeTeam = "Brighton", AwayTeam = "Crystal Palace", HomeGoalsAvg = 1.3f, AwayGoalsAvg = 1.4f, HomeGoalsConcededAvg = 1.1f, AwayGoalsConcededAvg = 1.2f, Outcome = "Draw" },
            new MatchDataV1 { HomeTeam = "Burnley", AwayTeam = "Fulham", HomeGoalsAvg = 1.0f, AwayGoalsAvg = 1.6f, HomeGoalsConcededAvg = 1.4f, AwayGoalsConcededAvg = 1.3f, Outcome = "HomeWin" }
        };
    }

    public List<MatchDataV2> LoadAndPrepareDataV3()
    {
        return new List<MatchDataV2>
    {
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 13),
            HostHomeTeam = "Liverpool",
            AwayTeam = "Chelsea",
            HostHomeGoalsAvg = 2.5f,
            HostAwayGoalsAvg = 1.5f,
            HostHomeGoalsConcededAvg = 1.2f,
            HostAwayGoalsConcededAvg = 1.5f,
            Outcome = "HomeWin",
            HostWinsLast5 = 3,
            HostLossesLast5 = 1,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 2.4f,
            OpponentWinsLast5 = 2,
            OpponentLossesLast5 = 2,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.6f,
            OpponentHomeGoalsAvg = 1.8f,
            OpponentHomeGoalsConcededAvg = 1.2f,
            OpponentHomeWinsLast5 = 2,
            OpponentHomeLossesLast5 = 2,
            OpponentHomeDrawsLast5 = 1,
            OpponentHomeAverageGoalsScoredLast5 = 1.5f,
            OpponentAwayGoalsAvg = 1.2f,
            OpponentAwayGoalsConcededAvg = 1.4f,
            OpponentAwayWinsLast5 = 1,
            OpponentAwayLossesLast5 = 3,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 1.2f,
            HeadToHeadWinsHome = 5,
            HeadToHeadWinsAway = 2,
            HeadToHeadDraws = 3
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 14),
            HostHomeTeam = "Manchester City",
            AwayTeam = "Arsenal",
            HostHomeGoalsAvg = 3.0f,
            HostAwayGoalsAvg = 1.5f,
            HostHomeGoalsConcededAvg = 0.8f,
            HostAwayGoalsConcededAvg = 2.0f,
            Outcome = "HomeWin",
            HostWinsLast5 = 4,
            HostLossesLast5 = 0,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 3.2f,
            OpponentWinsLast5 = 1,
            OpponentLossesLast5 = 3,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.4f,
            OpponentHomeGoalsAvg = 1.9f,
            OpponentHomeGoalsConcededAvg = 1.5f,
            OpponentHomeWinsLast5 = 1,
            OpponentHomeLossesLast5 = 3,
            OpponentHomeDrawsLast5 = 1,
            OpponentHomeAverageGoalsScoredLast5 = 1.3f,
            OpponentAwayGoalsAvg = 1.6f,
            OpponentAwayGoalsConcededAvg = 1.8f,
            OpponentAwayWinsLast5 = 0,
            OpponentAwayLossesLast5 = 4,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 1.2f,
            HeadToHeadWinsHome = 4,
            HeadToHeadWinsAway = 3,
            HeadToHeadDraws = 2
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 15),
            HostHomeTeam = "Tottenham",
            AwayTeam = "Manchester United",
            HostHomeGoalsAvg = 1.5f,
            HostAwayGoalsAvg = 1.5f,
            HostHomeGoalsConcededAvg = 1.0f,
            HostAwayGoalsConcededAvg = 1.0f,
            Outcome = "Draw",
            HostWinsLast5 = 2,
            HostLossesLast5 = 2,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 1.8f,
            OpponentWinsLast5 = 2,
            OpponentLossesLast5 = 2,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.5f,
            OpponentHomeGoalsAvg = 1.7f,
            OpponentHomeGoalsConcededAvg = 1.3f,
            OpponentHomeWinsLast5 = 1,
            OpponentHomeLossesLast5 = 3,
            OpponentHomeDrawsLast5 = 1,
            OpponentHomeAverageGoalsScoredLast5 = 1.2f,
            OpponentAwayGoalsAvg = 1.6f,
            OpponentAwayGoalsConcededAvg = 1.4f,
            OpponentAwayWinsLast5 = 1,
            OpponentAwayLossesLast5 = 3,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 1.4f,
            HeadToHeadWinsHome = 3,
            HeadToHeadWinsAway = 2,
            HeadToHeadDraws = 3
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 16),
            HostHomeTeam = "Arsenal",
            AwayTeam = "Everton",
            HostHomeGoalsAvg = 2.2f,
            HostAwayGoalsAvg = 1.0f,
            HostHomeGoalsConcededAvg = 1.1f,
            HostAwayGoalsConcededAvg = 1.8f,
            Outcome = "HomeWin",
            HostWinsLast5 = 4,
            HostLossesLast5 = 1,
            HostDrawsLast5 = 0,
            HostAverageGoalsScoredLast5 = 2.5f,
            OpponentWinsLast5 = 1,
            OpponentLossesLast5 = 3,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.2f,
            OpponentHomeGoalsAvg = 1.5f,
            OpponentHomeGoalsConcededAvg = 1.4f,
            OpponentHomeWinsLast5 = 1,
            OpponentHomeLossesLast5 = 2,
            OpponentHomeDrawsLast5 = 2,
            OpponentHomeAverageGoalsScoredLast5 = 1.0f,
            OpponentAwayGoalsAvg = 1.0f,
            OpponentAwayGoalsConcededAvg = 1.3f,
            OpponentAwayWinsLast5 = 0,
            OpponentAwayLossesLast5 = 3,
            OpponentAwayDrawsLast5 = 2,
            OpponentAwayAverageGoalsScoredLast5 = 0.8f,
            HeadToHeadWinsHome = 5,
            HeadToHeadWinsAway = 1,
            HeadToHeadDraws = 2
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 17),
            HostHomeTeam = "Liverpool",
            AwayTeam = "West Ham",
            HostHomeGoalsAvg = 2.8f,
            HostAwayGoalsAvg = 1.2f,
            HostHomeGoalsConcededAvg = 1.0f,
            HostAwayGoalsConcededAvg = 1.5f,
            Outcome = "HomeWin",
            HostWinsLast5 = 5,
            HostLossesLast5 = 0,
            HostDrawsLast5 = 0,
            HostAverageGoalsScoredLast5 = 3.0f,
            OpponentWinsLast5 = 1,
            OpponentLossesLast5 = 4,
            OpponentDrawsLast5 = 0,
            OpponentAverageGoalsScoredLast5 = 1.0f,
            OpponentHomeGoalsAvg = 1.5f,
            OpponentHomeGoalsConcededAvg = 1.2f,
            OpponentHomeWinsLast5 = 1,
            OpponentHomeLossesLast5 = 3,
            OpponentHomeDrawsLast5 = 1,
            OpponentHomeAverageGoalsScoredLast5 = 1.3f,
            OpponentAwayGoalsAvg = 1.0f,
            OpponentAwayGoalsConcededAvg = 1.4f,
            OpponentAwayWinsLast5 = 0,
            OpponentAwayLossesLast5 = 4,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 0.9f,
            HeadToHeadWinsHome = 6,
            HeadToHeadWinsAway = 2,
            HeadToHeadDraws = 1
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 18),
            HostHomeTeam = "Manchester United",
            AwayTeam = "Leicester",
            HostHomeGoalsAvg = 1.9f,
            HostAwayGoalsAvg = 1.5f,
            HostHomeGoalsConcededAvg = 1.5f,
            HostAwayGoalsConcededAvg = 1.2f,
            Outcome = "AwayWin",
            HostWinsLast5 = 2,
            HostLossesLast5 = 2,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 1.5f,
            OpponentWinsLast5 = 3,
            OpponentLossesLast5 = 1,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.8f,
            OpponentHomeGoalsAvg = 1.2f,
            OpponentHomeGoalsConcededAvg = 1.3f,
            OpponentHomeWinsLast5 = 2,
            OpponentHomeLossesLast5 = 1,
            OpponentHomeDrawsLast5 = 2,
            OpponentHomeAverageGoalsScoredLast5 = 1.4f,
            OpponentAwayGoalsAvg = 1.0f,
            OpponentAwayGoalsConcededAvg = 1.5f,
            OpponentAwayWinsLast5 = 1,
            OpponentAwayLossesLast5 = 3,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 1.0f,
            HeadToHeadWinsHome = 3,
            HeadToHeadWinsAway = 4,
            HeadToHeadDraws = 2
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 19),
            HostHomeTeam = "Chelsea",
            AwayTeam = "Southampton",
            HostHomeGoalsAvg = 2.0f,
            HostAwayGoalsAvg = 1.3f,
            HostHomeGoalsConcededAvg = 1.0f,
            HostAwayGoalsConcededAvg = 1.4f,
            Outcome = "Draw",
            HostWinsLast5 = 2,
            HostLossesLast5 = 2,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 1.8f,
            OpponentWinsLast5 = 1,
            OpponentLossesLast5 = 3,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 1.2f,
            OpponentHomeGoalsAvg = 1.5f,
            OpponentHomeGoalsConcededAvg = 1.1f,
            OpponentHomeWinsLast5 = 1,
            OpponentHomeLossesLast5 = 2,
            OpponentHomeDrawsLast5 = 2,
            OpponentHomeAverageGoalsScoredLast5 = 1.4f,
            OpponentAwayGoalsAvg = 1.0f,
            OpponentAwayGoalsConcededAvg = 1.5f,
            OpponentAwayWinsLast5 = 0,
            OpponentAwayLossesLast5 = 3,
            OpponentAwayDrawsLast5 = 2,
            OpponentAwayAverageGoalsScoredLast5 = 1.1f,
            HeadToHeadWinsHome = 3,
            HeadToHeadWinsAway = 2,
            HeadToHeadDraws = 2
        },
        new MatchDataV2
        {
            DateOne = new DateTime(2023, 8, 20),
            HostHomeTeam = "Aston Villa",
            AwayTeam = "Newcastle",
            HostHomeGoalsAvg = 1.5f,
            HostAwayGoalsAvg = 2.0f,
            HostHomeGoalsConcededAvg = 1.2f,
            HostAwayGoalsConcededAvg = 1.8f,
            Outcome = "AwayWin",
            HostWinsLast5 = 1,
            HostLossesLast5 = 3,
            HostDrawsLast5 = 1,
            HostAverageGoalsScoredLast5 = 1.2f,
            OpponentWinsLast5 = 3,
            OpponentLossesLast5 = 1,
            OpponentDrawsLast5 = 1,
            OpponentAverageGoalsScoredLast5 = 2.1f,
            OpponentHomeGoalsAvg = 1.8f,
            OpponentHomeGoalsConcededAvg = 1.5f,
            OpponentHomeWinsLast5 = 2,
            OpponentHomeLossesLast5 = 2,
            OpponentHomeDrawsLast5 = 1,
            OpponentHomeAverageGoalsScoredLast5 = 1.7f,
            OpponentAwayGoalsAvg = 2.2f,
            OpponentAwayGoalsConcededAvg = 1.4f,
            OpponentAwayWinsLast5 = 2,
            OpponentAwayLossesLast5 = 2,
            OpponentAwayDrawsLast5 = 1,
            OpponentAwayAverageGoalsScoredLast5 = 1.5f,
            HeadToHeadWinsHome = 2,
            HeadToHeadWinsAway = 3,
            HeadToHeadDraws = 2
        }
    };
    }

    public ITransformer TrainModelV2old(List<MatchDataV1> data)
    {
        try
        {

            // Ensure that training data is not null or empty
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Training data cannot be null or empty.", nameof(data));
            }

            // Check for distinct outcomes
            var distinctOutcomes = data.Select(md => md.Outcome).Distinct().ToList();
            Console.WriteLine("Distinct Outcomes: " + string.Join(", ", distinctOutcomes));

            if (distinctOutcomes.Count < 2)
            {
                throw new ArgumentException("Training data must contain at least two distinct outcomes.", nameof(data));
            }
            var HomeWin = data.Where(x => x.Outcome == "HomeWin").ToList();
            var AwayWin = data.Where(x => x.Outcome == "AwayWin").ToList();
            var Draw = data.Where(x => x.Outcome == "Draw").ToList();

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(MatchDataV1.Outcome))
            .Append(mlContext.Transforms.Concatenate("Features",
                nameof(MatchDataV1.HomeGoalsAvg),
                nameof(MatchDataV1.AwayGoalsAvg),
                nameof(MatchDataV1.HomeGoalsConcededAvg),
                nameof(MatchDataV1.AwayGoalsConcededAvg),
                nameof(MatchDataV1.HomeWinsLast5),
                nameof(MatchDataV1.HomeLossesLast5),
                nameof(MatchDataV1.HomeDrawsLast5),
                nameof(MatchDataV1.HomeAverageGoalsScoredLast5),
                nameof(MatchDataV1.AwayWinsLast5),
                nameof(MatchDataV1.AwayLossesLast5),
                nameof(MatchDataV1.AwayDrawsLast5),
                nameof(MatchDataV1.AwayAverageGoalsScoredLast5),
                 nameof(MatchDataV1.HST),
                nameof(MatchDataV1.AST),
                nameof(MatchDataV1.HeadToHeadWinsHome),
                nameof(MatchDataV1.HeadToHeadWinsAway),
                nameof(MatchDataV1.HeadToHeadDraws))

            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features")));
            //.Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features")));
            //var test = data.Take(40).ToList();
            data = LoadAndPrepareData();
            //data.AddRange(data);
            var trainingData = mlContext.Data.LoadFromEnumerable(data);
            return pipeline.Fit(trainingData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while training the model: {ex.Message}");
            throw;
        }
    }
    public void OutComeDistribution(List<MatchDataV1> matchDataList)
    {
        var outcomeCounts = matchDataList.GroupBy(md => md.Outcome)
                         .Select(g => new { Outcome = g.Key, Count = g.Count() })
                         .ToList();

        foreach (var count in outcomeCounts)
        {
            Console.WriteLine($"Outcome: {count.Outcome}, Count: {count.Count}");
        }
    }

    public void OutComeDistribution2(List<MatchDataV2> matchDataList)
    {
        var outcomeCounts = matchDataList.GroupBy(md => md.Outcome)
                         .Select(g => new { Outcome = g.Key, Count = g.Count() })
                         .ToList();

        foreach (var count in outcomeCounts)
        {
            Console.WriteLine($"Outcome: {count.Outcome}, Count: {count.Count}");
        }
    }
    public List<MatchDataV1> duplicateDraws(List<MatchDataV1> matchDataList)
    {
        var draws = matchDataList.Where(md => md.Outcome == "Draw").ToList();
        var drawDuplicates = draws.Select(draw => new MatchDataV1
        {
            HomeTeam = draw.HomeTeam,
            AwayTeam = draw.AwayTeam,
            HomeGoalsAvg = draw.HomeGoalsAvg,
            AwayGoalsAvg = draw.AwayGoalsAvg,
            HomeGoalsConcededAvg = draw.HomeGoalsConcededAvg,
            AwayGoalsConcededAvg = draw.AwayGoalsConcededAvg,
            Outcome = draw.Outcome
        }).ToList();
        var drawsToRemove = drawDuplicates.Take(6500).ToList(); // Math.Min(280, drawDuplicates.Count);
        matchDataList = drawDuplicates.Except(drawsToRemove).ToList();
        return matchDataList;
    }

    public ITransformer TrainModelV2(List<MatchDataV1> data)
    {
        var mlContext = new MLContext();

        try
        {
            // Ensure that training data is not null or empty
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Training data cannot be null or empty.", nameof(data));
            }

            // Check for distinct outcomes
            var distinctOutcomes = data.Select(md => md.Outcome).Distinct().ToList();
            if (distinctOutcomes.Count < 2)
            {
                throw new ArgumentException("Training data must contain at least two distinct outcomes.", nameof(data));
            }

            // Load training data into IDataView
            //var take50 = data.Take(20).ToList();
        
            var distinctMatches = data
                              .GroupBy(m => new { m.HomeTeam, m.AwayTeam })
                              .Select(g => g.First())
                              .ToList();
            data = distinctMatches;
            //data = new List<MatchDataV1>();
            //var draw = distinctMatches.OrderByDescending(x => x.Date).Where(x => x.Outcome == "Draw").Take(150).ToList();
            //var AwayWin = distinctMatches.OrderByDescending(x => x.Date).Where(x => x.Outcome == "AwayWin").Take(150).ToList();
            //var HomeWin = distinctMatches.OrderByDescending(x => x.Date).Where(x => x.Outcome == "HomeWin").Take(150).ToList();

            //data.AddRange(draw);
            //data.AddRange(HomeWin);
            //data.AddRange(AwayWin);

            var trainingData = mlContext.Data.LoadFromEnumerable(data);

            // Define the pipeline
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(MatchDataV1.Outcome))
                .Append(mlContext.Transforms.Concatenate("Features",
                    nameof(MatchDataV1.HomeGoalsAvg),
                    nameof(MatchDataV1.AwayGoalsAvg),
                    nameof(MatchDataV1.HomeGoalsConcededAvg),
                    nameof(MatchDataV1.AwayGoalsConcededAvg),

                    nameof(MatchDataV1.HomeWinsLast5),
                    nameof(MatchDataV1.HomeLossesLast5),
                    nameof(MatchDataV1.HomeDrawsLast5),
                    nameof(MatchDataV1.HomeAverageGoalsScoredLast5),
                    nameof(MatchDataV1.AwayWinsLast5),
                    nameof(MatchDataV1.AwayLossesLast5),
                    nameof(MatchDataV1.AwayDrawsLast5),
                    nameof(MatchDataV1.AwayAverageGoalsScoredLast5),
                    // nameof(MatchDataV1.HST),
                    //nameof(MatchDataV1.AST),
                    nameof(MatchDataV1.HeadToHeadWinsHome),
                    nameof(MatchDataV1.HeadToHeadWinsAway),
                    nameof(MatchDataV1.HeadToHeadDraws))

                )
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"));

            // Fit the model
            var model = pipeline.Fit(trainingData);
            return model; // Return the trained model
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while training the model: {ex.Message}");
            throw;
        }
    }
    public DateTime ConvertUnixTimestampToDateTime(long unixTimestamp)
    {
        // Convert Unix timestamp to DateTime
        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        return dateTime;
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
            var date = ConvertUnixTimestampToDateTime(item.Date);
            string output = $"{date},{item.HomeTeam},{item.AwayTeam}," +
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

    public void fileOutput(List<MatchDataV1> data)
    {
        string filePath = "C:\\Users\\Godfrey.Masha\\Downloads\\modalOutput.txt";

        foreach (var item in data)
        {
            string output = $"\n\n" +
                        $"HomeTeam: {item.HomeTeam}\n" +
                        $"AwayTeam: {item.AwayTeam}\n" +
                        $"HomeGoalsAvg: {item.HomeGoalsAvg}\n" +
                        $"AwayGoalsAvg: {item.AwayGoalsAvg}\n" +
                        $"HomeGoalsConcededAvg: {item.HomeGoalsConcededAvg}\n" +
                        $"AwayGoalsConcededAvg: {item.AwayGoalsConcededAvg}\n" +
                        $"HomeWinsLast5: {item.HomeWinsLast5}\n" +
                        $"HomeLossesLast5: {item.HomeLossesLast5}\n" +
                        $"HomeDrawsLast5: {item.HomeDrawsLast5}\n" +
                        $"HomeAverageGoalsScoredLast5: {item.HomeAverageGoalsScoredLast5}\n" +
                        $"AwayWinsLast5: {item.AwayWinsLast5}\n" +
                        $"AwayLossesLast5: {item.AwayLossesLast5}\n" +
                        $"AwayDrawsLast5: {item.AwayDrawsLast5}\n" +
                        $"AwayAverageGoalsScoredLast5: {item.AwayAverageGoalsScoredLast5}\n" +
                        $"HST: {item.HST}\n" +
                        $"AST: {item.AST}\n" +
                        $"HeadToHeadWinsHome: {item.HeadToHeadWinsHome}\n" +
                        $"HeadToHeadWinsAway: {item.HeadToHeadWinsAway}\n" +
                        $"HeadToHeadDraws: {item.HeadToHeadDraws}\n" +
                        $"Outcome: {item.Outcome}\n";

            File.AppendAllTextAsync(filePath, output);

        }


    }

    public ITransformer TrainModelV3(List<MatchDataV2> data)
    {
        var mlContext = new MLContext();

        try
        {
            // Ensure that training data is not null or empty
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Training data cannot be null or empty.", nameof(data));
            }

            // Check for distinct outcomes
            var distinctOutcomes = data.Select(md => md.Outcome).Distinct().ToList();
            if (distinctOutcomes.Count < 2)
            {
                throw new ArgumentException("Training data must contain at least two distinct outcomes.", nameof(data));
            }

            // Load training data into IDataView
            //var take50 = data.Take(20).ToList();
            //data = data.GroupBy(m => new { m.HostHomeTeam, m.AwayTeam })
            //            .Select(g => g.First())
            //            .ToList();
            var draw = data.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "Draw").Take(175).ToList();
            var AwayWin = data.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "AwayWin").Take(175).ToList();
            var HomeWin = data.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "HomeWin").Take(175).ToList();

            //data = LoadAndPrepareDataV3();
            data = new List<MatchDataV2>();
            data.AddRange(draw);
            data.AddRange(AwayWin);
            data.AddRange(HomeWin);
            var distinctMatches = data
                              .GroupBy(m => new { m.HostHomeTeam, m.AwayTeam })
                              .Select(g => g.First())
                              .ToList();
            //data = distinctMatches;
            //OutComeDistribution(data);

            var trainingData = mlContext.Data.LoadFromEnumerable(data);
            // Define the pipeline
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(MatchDataV2.Outcome))
                .Append(mlContext.Transforms.Concatenate("Features",
                    nameof(MatchDataV2.HeadToHeadDraws),
                    nameof(MatchDataV2.HeadToHeadWinsAway),
                    nameof(MatchDataV2.HeadToHeadWinsHome),

                    //nameof(MatchDataV2.HostAverageGoalsScoredLast5),
                    //nameof(MatchDataV2.OpponentAverageGoalsScoredLast5),
                    //nameof(MatchDataV2.HostDrawsLast5),
                    //nameof(MatchDataV2.OpponentDrawsLast5),
                    //nameof(MatchDataV2.HostLossesLast5), //working
                    //nameof(MatchDataV2.OpponentLossesLast5),
                    nameof(MatchDataV2.HostWinsLast5), //working
                    nameof(MatchDataV2.OpponentWinsLast5),

                    //nameof(MatchDataV2.HostAwayAverageGoalsScoredLast5),
                    //nameof(MatchDataV2.OpponentAwayAverageGoalsScoredLast5),
                    nameof(MatchDataV2.HostAwayDrawsLast5), //working
                    nameof(MatchDataV2.OpponentAwayDrawsLast5),

                    //nameof(MatchDataV2.HostAwayGoalsAvg),
                    //nameof(MatchDataV2.OpponentAwayGoalsAvg),
                    //nameof(MatchDataV2.HostAwayGoalsConcededAvg),
                    //nameof(MatchDataV2.OpponentAwayGoalsConcededAvg),

                    //nameof(MatchDataV2.HostAwayLossesLast5),
                    //nameof(MatchDataV2.OpponentAwayLossesLast5),
                    //nameof(MatchDataV2.HostAwayWinsLast5),
                    //nameof(MatchDataV2.OpponentAwayWinsLast5),
                    //nameof(MatchDataV2.HostDrawsLast5),
                    //nameof(MatchDataV2.OpponentDrawsLast5),

                    //nameof(MatchDataV2.HostHomeAverageGoalsScoredLast5),
                    //nameof(MatchDataV2.OpponentHomeAverageGoalsScoredLast5),
                    nameof(MatchDataV2.HostHomeDrawsLast5), //working
                    nameof(MatchDataV2.OpponentHomeDrawsLast5),
                    //nameof(MatchDataV2.HostHomeGoalsAvg),
                    //nameof(MatchDataV2.OpponentHomeGoalsAvg),

                    nameof(MatchDataV2.HostHomeGoalsConcededAvg),
                    nameof(MatchDataV2.OpponentHomeGoalsConcededAvg),
                    nameof(MatchDataV2.HostHomeLossesLast5),
                    nameof(MatchDataV2.OpponentHomeLossesLast5),
                    nameof(MatchDataV2.HostHomeWinsLast5),
                    nameof(MatchDataV2.OpponentHomeWinsLast5),

                    nameof(MatchDataV2.HostLossesLast5),
                    nameof(MatchDataV2.OpponentLossesLast5),
                    nameof(MatchDataV2.HostWinsLast5),
                    nameof(MatchDataV2.OpponentWinsLast5))
                    )
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"));

            // Fit the model
            var model = pipeline.Fit(trainingData);
            return model; // Return the trained model
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while training the model: {ex.Message}");
            throw;
        }
    }

    public void PredictMatchOutcomeV3(ITransformer model, string teamA, string teamB, List<MatchDataV2> matchDataList)
    {
        //matchDataList.AddRange(duplicateDraws(matchDataList));
        var distinctMatches = matchDataList
                               .GroupBy(m => new { m.HostHomeTeam, m.AwayTeam })
                               .Select(g => g.First())
                               .ToList();

        Console.WriteLine($"Predicting outcome for: {teamA} vs {teamB}");
        matchDataList = distinctMatches;
        var draw = matchDataList.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "Draw").Take(175).ToList();
        var AwayWin = matchDataList.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "AwayWin").Take(175).ToList();
        var HomeWin = matchDataList.OrderByDescending(x => x.DateOne).Where(x => x.Outcome == "HomeWin").Take(175).ToList();

        //matchDataList = LoadAndPrepareDataV3();
        matchDataList = new List<MatchDataV2>();
        matchDataList.AddRange(draw);
        matchDataList.AddRange(AwayWin);
        matchDataList.AddRange(HomeWin);
        var input = matchDataList.FirstOrDefault(m => m.HostHomeTeam == teamA && m.AwayTeam == teamB);

     
        OutComeDistribution2(matchDataList);

        //FileOutputCsvV3(matchDataList);

        if (input != null || input == null)
        {
            // Initialize counters for metrics
            int total = 0;
            int correctPredictions = 0;
            int truePositivesHomeWin = 0;
            int truePositivesDraw = 0;
            int truePositivesAwayWin = 0;
            int falsePositivesHomeWin = 0;
            int falsePositivesDraw = 0;
            int falsePositivesAwayWin = 0;
            foreach (var item in matchDataList)
            {
                input = item;
                var predictionEngine = mlContext.Model.CreatePredictionEngine<MatchDataV2, Prediction>(model);
                var prediction = predictionEngine.Predict(input);
                string predictedOutcome = prediction.Outcome switch
                {
                    1 => "HomeWin",
                    2 => "Draw",
                    3 => "AwayWin",
                    _ => "Unknown"
                };
                Console.WriteLine($"{input.HostHomeTeam} vs {input.AwayTeam}: actual outcome: {item.Outcome} outcome: {predictedOutcome}");
                // Compare with actual outcome
                total++;
                if (predictedOutcome == item.Outcome)
                {
                    correctPredictions++;
                }

                // Calculate true positives and false positives
                if (predictedOutcome == "HomeWin")
                {
                    if (item.Outcome == "HomeWin") truePositivesHomeWin++;
                    else falsePositivesHomeWin++;
                }
                else if (predictedOutcome == "Draw")
                {
                    if (item.Outcome == "Draw") truePositivesDraw++;
                    else falsePositivesDraw++;
                }
                else if (predictedOutcome == "AwayWin")
                {
                    if (item.Outcome == "AwayWin") truePositivesAwayWin++;
                    else falsePositivesAwayWin++;
                }
            }
            // Calculate accuracy
            double accuracy = (double)correctPredictions / total;

            // Calculate precision, recall, and F1 score for each class
            double precisionHomeWin = (double)truePositivesHomeWin / (truePositivesHomeWin + falsePositivesHomeWin);
            double recallHomeWin = (double)truePositivesHomeWin / (truePositivesHomeWin + (total - truePositivesHomeWin)); // Adjust as necessary
            double f1HomeWin = 2 * (precisionHomeWin * recallHomeWin) / (precisionHomeWin + recallHomeWin);

            double precisionDraw = (double)truePositivesDraw / (truePositivesDraw + falsePositivesDraw);
            double recallDraw = (double)truePositivesDraw / (truePositivesDraw + (total - truePositivesDraw)); // Adjust as necessary
            double f1Draw = 2 * (precisionDraw * recallDraw) / (precisionDraw + recallDraw);

            double precisionAwayWin = (double)truePositivesAwayWin / (truePositivesAwayWin + falsePositivesAwayWin);
            double recallAwayWin = (double)truePositivesAwayWin / (truePositivesAwayWin + (total - truePositivesAwayWin)); // Adjust as necessary
            double f1AwayWin = 2 * (precisionAwayWin * recallAwayWin) / (precisionAwayWin + recallAwayWin);

            // Output the results
            Console.WriteLine($"Total Matches: {total}");
            Console.WriteLine($"Correct Predictions: {correctPredictions}");
            Console.WriteLine($"Accuracy: {accuracy:P2}");
            Console.WriteLine($"HomeWin - Precision: {precisionHomeWin:P2}, Recall: {recallHomeWin:P2}, F1: {f1HomeWin:P2}");
            Console.WriteLine($"Draw - Precision: {precisionDraw:P2}, Recall: {recallDraw:P2}, F1: {f1Draw:P2}");
            Console.WriteLine($"AwayWin - Precision: {precisionAwayWin:P2}, Recall: {recallAwayWin:P2}, F1: {f1AwayWin:P2}");
        }
        else
        {
            Console.WriteLine("No data available for the specified teams.");
        }
    }
    public void PredictMatchOutcomeV2(ITransformer model, string teamA, string teamB, List<MatchDataV1> matchDataList)
    {
        //matchDataList.AddRange(duplicateDraws(matchDataList));
        var distinctMatches = matchDataList
                               .GroupBy(m => new { m.HomeTeam, m.AwayTeam })
                               .Select(g => g.First())
                               .ToList();

        Console.WriteLine($"Predicting outcome for: {teamA} vs {teamB}");
   
        matchDataList = distinctMatches;
        var input = matchDataList.FirstOrDefault(m => m.HomeTeam == teamA && m.AwayTeam == teamB);
        fileOutput(matchDataList);
        FileOutputCsv(matchDataList);
        OutComeDistribution(matchDataList);

        if (input != null || input == null)
        {
            // Initialize counters for metrics
            int total = 0;
            int correctPredictions = 0;
            int truePositivesHomeWin = 0;
            int truePositivesDraw = 0;
            int truePositivesAwayWin = 0;
            int falsePositivesHomeWin = 0;
            int falsePositivesDraw = 0;
            int falsePositivesAwayWin = 0;
            foreach (var item in matchDataList)
            {
                input = item;
                var predictionEngine = mlContext.Model.CreatePredictionEngine<MatchDataV1, Prediction>(model);
                var prediction = predictionEngine.Predict(input);
                string predictedOutcome = prediction.Outcome switch
                {
                    1 => "HomeWin",
                    2 => "Draw",
                    3 => "AwayWin",
                    _ => "Unknown"
                };
                Console.WriteLine($"{input.HomeTeam} vs {input.AwayTeam}: actual outcome: {item.Outcome} outcome: {predictedOutcome}");
                // Compare with actual outcome
                total++;
                if (predictedOutcome == item.Outcome)
                {
                    correctPredictions++;
                }

                // Calculate true positives and false positives
                if (predictedOutcome == "HomeWin")
                {
                    if (item.Outcome == "HomeWin") truePositivesHomeWin++;
                    else falsePositivesHomeWin++;
                }
                else if (predictedOutcome == "Draw")
                {
                    if (item.Outcome == "Draw") truePositivesDraw++;
                    else falsePositivesDraw++;
                }
                else if (predictedOutcome == "AwayWin")
                {
                    if (item.Outcome == "AwayWin") truePositivesAwayWin++;
                    else falsePositivesAwayWin++;
                }
            }
            // Calculate accuracy
            double accuracy = (double)correctPredictions / total;

            // Calculate precision, recall, and F1 score for each class
            double precisionHomeWin = (double)truePositivesHomeWin / (truePositivesHomeWin + falsePositivesHomeWin);
            double recallHomeWin = (double)truePositivesHomeWin / (truePositivesHomeWin + (total - truePositivesHomeWin)); // Adjust as necessary
            double f1HomeWin = 2 * (precisionHomeWin * recallHomeWin) / (precisionHomeWin + recallHomeWin);

            double precisionDraw = (double)truePositivesDraw / (truePositivesDraw + falsePositivesDraw);
            double recallDraw = (double)truePositivesDraw / (truePositivesDraw + (total - truePositivesDraw)); // Adjust as necessary
            double f1Draw = 2 * (precisionDraw * recallDraw) / (precisionDraw + recallDraw);

            double precisionAwayWin = (double)truePositivesAwayWin / (truePositivesAwayWin + falsePositivesAwayWin);
            double recallAwayWin = (double)truePositivesAwayWin / (truePositivesAwayWin + (total - truePositivesAwayWin)); // Adjust as necessary
            double f1AwayWin = 2 * (precisionAwayWin * recallAwayWin) / (precisionAwayWin + recallAwayWin);

            // Output the results
            Console.WriteLine($"Total Matches: {total}");
            Console.WriteLine($"Correct Predictions: {correctPredictions}");
            Console.WriteLine($"Accuracy: {accuracy:P2}");
            Console.WriteLine($"HomeWin - Precision: {precisionHomeWin:P2}, Recall: {recallHomeWin:P2}, F1: {f1HomeWin:P2}");
            Console.WriteLine($"Draw - Precision: {precisionDraw:P2}, Recall: {recallDraw:P2}, F1: {f1Draw:P2}");
            Console.WriteLine($"AwayWin - Precision: {precisionAwayWin:P2}, Recall: {recallAwayWin:P2}, F1: {f1AwayWin:P2}");
        }
        else
        {
            Console.WriteLine("No data available for the specified teams.");
        }
    }
    public string PredictOutcome(MatchData match)
    {
        try
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MatchData, Prediction>(model);
            var prediction = predictionEngine.Predict(match);

            // Convert the predicted key to a string outcome
            return prediction.Outcome switch
            {
                0 => "HomeWin",
                1 => "AwayWin",
                2 => "Draw",
                _ => "Unknown"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during prediction: {ex.Message}");
            throw;
        }
    }

    //public void TrainModel(IEnumerable<MatchData> trainingData)
    //{
    //    var trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);

    //    // Define the training pipeline
    //    var pipeline = mlContext.Transforms.Concatenate("Features", "HomeScore", "AwayScore")
    //                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Outcome", maximumNumberOfIterations: 100))
    //                .Append(mlContext.Transforms.Conversion.MapValueToKey("Outcome"));

    //    // Train the model
    //    model = pipeline.Fit(trainingDataView);
    //}

    //public string PredictOutcome(MatchData match)
    //{
    //    var predictionEngine = mlContext.Model.CreatePredictionEngine<MatchData, Prediction>(model);
    //    var prediction = predictionEngine.Predict(match);
    //    return prediction.Outcome;
    //}
}
