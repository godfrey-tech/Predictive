using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class MatchOutcomePredictor : IMatchOutcomePredictor
    {
        private readonly ISeasonStatsService _seasonStatsService;
        private readonly IHeadToHeadStatsService _headToHeadStatsService;

        public MatchOutcomePredictor(ISeasonStatsService seasonStatsService, IHeadToHeadStatsService headToHeadStatsService)
        {
            _seasonStatsService = seasonStatsService;
            _headToHeadStatsService = headToHeadStatsService;
        }

        public PredictOutcome PredictOutcome(string homeTeam, string awayTeam, DateTime startSeason, DateTime endSeasonD)
        {
            // Define the current date
            DateTime currentDate = DateTime.Now;

            // Calculate the end date of the current season
            DateTime endSeason = new DateTime(currentDate.Year, 5, 31);
            // Calculate the start date for the recent data
            DateTime recentStartSeason = endSeason.AddYears(-2).AddMonths(-7); // Start of the recent season (August)
            var homeStatsRecent = GetTeamStatistics(homeTeam, recentStartSeason, endSeason);
            var awayStatsRecent = GetTeamStatistics(awayTeam, recentStartSeason, endSeason);

            // Calculate probabilities for recent data
            double homeWinProbRecent = CalculateWinProbability(homeStatsRecent);
            double awayWinProbRecent = CalculateWinProbability(awayStatsRecent);
            double drawProbRecent = CalculateDrawProbability(homeStatsRecent, awayStatsRecent);

            // Gather statistics for the last 10 years
            DateTime tenYearsAgo = currentDate.AddYears(-10);
            var homeStatsHistorical = GetTeamStatistics(homeTeam, tenYearsAgo, endSeason);
            var awayStatsHistorical = GetTeamStatistics(awayTeam, tenYearsAgo, endSeason);

            // Calculate probabilities for historical data
            double homeWinProbHistorical = CalculateWinProbability(homeStatsHistorical);
            double awayWinProbHistorical = CalculateWinProbability(awayStatsHistorical);
            double drawProbHistorical = CalculateDrawProbability(homeStatsHistorical, awayStatsHistorical);

            // Gather recent form for both teams
            double homeRecentForm = homeStatsRecent.RecentForm;
            double awayRecentForm = awayStatsRecent.RecentForm;
            double homeWinStreak = homeStatsRecent.WinStreak;
            double awayWinStreak = awayStatsRecent.WinStreak;

            // Weighting factors
            double recentWeight = 0.5; // 50% weight for recent data
            double historicalWeight = 0.3; // 30% weight for historical data
            double recentFormWeight = 0.15; // 05% weight for recent form
            double winStreakWeight = 0.05; // 15% weight for win streak

            // Combine probabilities
            double homeWinProb = (homeWinProbRecent * recentWeight)
                                + (homeWinProbHistorical * historicalWeight)
                                + (homeRecentForm * recentFormWeight)
                                + (homeWinStreak * winStreakWeight); // Include win streak

            double awayWinProb = (awayWinProbRecent * recentWeight)
                                + (awayWinProbHistorical * historicalWeight)
                                + (awayRecentForm * recentFormWeight)
                                + (awayWinStreak * winStreakWeight); // Include win streak

            double drawProb = (drawProbRecent * recentWeight)
                             + (drawProbHistorical * historicalWeight);
            // Make prediction based on the highest probability
            var total = homeWinProb + awayWinProb + drawProb;
            var homeWinPercentage = (homeWinProb / total) * 100;
            var awayWinPercentage = (awayWinProb / total) * 100;
            var drawPercentage = (drawProb / total) * 100;

            double homeDifference = Math.Abs(homeWinPercentage - awayWinPercentage);
            //double awayDifference = awayWinProb - homeWinProb;
            string category = "";
            if ((homeWinPercentage >= 60 || awayWinPercentage >= 60 || drawPercentage >= 60) && homeDifference >= 20)
            {
                category = "Low Risk";
            }
            else if ((homeWinPercentage >= 50 || awayWinPercentage >= 50 || drawPercentage >= 50) && homeDifference >= 15)
            {
                category = "Moderate Risk";
            }
            else if ((homeWinPercentage >= 38 || awayWinPercentage >= 38 || drawPercentage >= 38) && homeDifference >= 10)
            {
                category = "High Risk";
            }
            else if ((homeWinPercentage >= 30 || awayWinPercentage >= 30 || drawPercentage >= 30) && homeDifference >= 5)
            {
                category = "Very High Risk";
            }
            else 
            {
                category = "No bet";

            }
            var goalStats = this.CalculateOverGoalsProbabilities(homeTeam, awayTeam);
            return new PredictOutcome()
            {
                HomeWinProbability = homeWinPercentage,
                AwayWinProbability = awayWinPercentage,
                DrawProbability = drawPercentage,
                DifferenceProbability = homeDifference,
                HomeAverageGoalsScored = homeStatsRecent.AverageGoalsScored,
                AwayAverageGoalsScored = awayStatsRecent.AverageGoalsScored,
                HomeHistoricalAverageGoalsScored = homeStatsHistorical.AverageGoalsScored,
                AwayHistoricalAverageGoalsScored = awayStatsHistorical.AverageGoalsScored,
                Over1GoalsProbability = goalStats.Over1GoalsProbability,
                Over2GoalsProbability = goalStats.Over2GoalsProbability,
                Category = category,  
                Outcome = MakePrediction(homeWinProb, awayWinProb, drawProb)
            };
            //return MakePrediction(homeWinProb, awayWinProb, drawProb);
        }

        public GoalStatistics CalculateOverGoalsProbabilities(string teamA, string teamB)
        {
            // Define the current date
            DateTime currentDate = DateTime.Now;
            // Calculate the end date of the current season
            DateTime endSeason = new DateTime(currentDate.Year, 5, 31);
            // Calculate the start date for the recent data
            DateTime recentStartSeason = endSeason.AddYears(-2).AddMonths(-7); // Start of the recent season (August)

            // Get recent averages
            double recentAverageGoalsA = _seasonStatsService.GetAverageGoalsScored(teamA, recentStartSeason, endSeason);
            double recentAverageGoalsB = _seasonStatsService.GetAverageGoalsScored(teamB, recentStartSeason, endSeason);

            // Get historical averages
            DateTime tenYearsAgo = currentDate.AddYears(-10);
            double historicalAverageGoalsA = _seasonStatsService.GetAverageGoalsScored(teamA, tenYearsAgo, endSeason);
            double historicalAverageGoalsB = _seasonStatsService.GetAverageGoalsScored(teamB, tenYearsAgo, endSeason);

            // Get head-to-head averages
            double headToHeadAverageGoals = _headToHeadStatsService.GetHeadToHeadAverageGoals(teamA, teamB, recentStartSeason, endSeason);

            // Combine averages with weights (weights can be adjusted)
            double weightRecent = 0.5;  // 50% weight for recent averages
            double weightHistorical = 0.1; // 20% weight for historical averages
            double weightHeadToHead = 0.4; // 30% weight for head-to-head averages

            double combinedAverage = (recentAverageGoalsA * weightRecent + recentAverageGoalsB * weightRecent +
                                      historicalAverageGoalsA * weightHistorical + historicalAverageGoalsB * weightHistorical +
                                      headToHeadAverageGoals * weightHeadToHead) /
                                     (2 * weightRecent + 2 * weightHistorical + weightHeadToHead); // Normalizing the weights

            // Calculate probabilities for over 1 and over 2 goals
            double over1GoalsProbability = (1 - PoissonProbability(combinedAverage, 1)) * 100;
            double over2GoalsProbability = (1 - PoissonProbability(combinedAverage, 2)) * 100;

            return new GoalStatistics
            {
                Over1GoalsProbability = over1GoalsProbability,
                Over2GoalsProbability = over2GoalsProbability   
            };
        }

        private double PoissonProbability(double average, int threshold)
        {
            double probability = 0;
            for (int i = 0; i <= threshold; i++)
            {
                probability += (Math.Pow(average, i) * Math.Exp(-average)) / Factorial(i);
            }
            return probability;
        }

        private int Factorial(int number)
        {
            if (number == 0) return 1;
            return number * Factorial(number - 1);
        }
        public TeamStatistics GetTeamStatistics(string team, DateTime startSeason, DateTime endSeason)
        {
            int totalWins = _seasonStatsService.GetTotalWins(team, startSeason, endSeason);
            int totalDraws = _seasonStatsService.GetTotalDraw(team, startSeason, endSeason);
            int totalLosses = _seasonStatsService.GetTotalLosses(team, startSeason, endSeason);
            int totalMatches = totalWins + totalDraws + totalLosses;

            double averageGoalsScored = _seasonStatsService.GetAverageGoalsScored(team, startSeason, endSeason);
            double averageGoalsConceded = _seasonStatsService.GetAverageGoalsConceded(team, startSeason, endSeason);
            double goalDifference = averageGoalsScored - averageGoalsConceded;

            double recentForm = CalculateRecentForm(team, startSeason, endSeason);
            double homeAwayPerformance = CalculateHomeAwayPerformance(team, startSeason, endSeason);
            double winStreak = CalculateWinStreak(team, startSeason, endSeason);

            return new TeamStatistics
            {
                TotalWins = totalWins,
                TotalDraws = totalDraws,
                TotalLosses = totalLosses,
                TotalMatches = totalMatches,
                AverageGoalsScored = averageGoalsScored,
                AverageGoalsConceded = averageGoalsConceded,
                GoalDifference = goalDifference,
                RecentForm = recentForm,
                HomeAwayPerformance = homeAwayPerformance,
                WinStreak = winStreak
            };
        }

        private double CalculateRecentForm(string team, DateTime startSeason, DateTime endSeason)
        {
            var recentMatches = _seasonStatsService.GetRecentMatches(team, startSeason, endSeason, 5);
            if (recentMatches.Count == 0)
            {
                return 0.0; // or return a default value, or consider using null for better handling
            }

            int wins = recentMatches.Count(x => (x.HomeTeam == team && x.HomeGoals > x.AwayGoals) ||
                                                 (x.AwayTeam == team && x.AwayGoals > x.HomeGoals));
            int draws = recentMatches.Count(x => (x.HomeTeam == team && x.HomeGoals == x.AwayGoals) ||
                                                 (x.AwayTeam == team && x.AwayGoals == x.HomeGoals));
            int totalMatches = recentMatches.Count;

            // Weighted recent form score
            double recentFormScore = (wins * 3 + draws) / (double)(totalMatches * 3);


            return recentFormScore;
        }


        private double CalculateHomeAwayPerformance(string team, DateTime startSeason, DateTime endSeason)
        {
            // Calculate performance based on home vs away matches
            var homeMatches = _seasonStatsService.GetHomeMatches(team, startSeason, endSeason);
            var awayMatches = _seasonStatsService.GetAwayMatches(team, startSeason, endSeason);
            double homeWinRate = (double)homeMatches.Count(x => x.HomeGoals > x.AwayGoals) / homeMatches.Count();
            double awayWinRate = (double)awayMatches.Count(x => x.AwayGoals > x.HomeGoals) / awayMatches.Count();
            return (homeWinRate + awayWinRate) / 2; // Average performance
        }

        private double CalculateWinStreak(string teamName, DateTime startSeason, DateTime matchDate)
        {
            // Calculate the length of the current win streak
            var recentMatches = _seasonStatsService.GetRecentMatches(teamName, startSeason, matchDate, 5);
            int streak = 0;
            foreach (var match in recentMatches)
            {
                // Check if the team won the match
                if ((match.HomeTeam == teamName && match.HomeGoals > match.AwayGoals) ||
                    (match.AwayTeam == teamName && match.AwayGoals > match.HomeGoals))
                {
                    streak++;
                }
                else
                {
                    break; // Stop counting if a loss or draw is encountered
                }
            }
            return streak;
        }
        private double CalculateWinProbability(TeamStatistics stats)
        {
            return stats.TotalMatches > 0 ? (double)stats.TotalWins / stats.TotalMatches : 0;
        }

        private double CalculateDrawProbability(TeamStatistics homeStats, TeamStatistics awayStats)
        {
            // A simple average of historical draw rates for both teams
            return (double)(homeStats.TotalDraws + awayStats.TotalDraws) /
                   (homeStats.TotalMatches + awayStats.TotalMatches);
        }

        private string MakePrediction(double homeWinProb, double awayWinProb, double drawProb)
        {
            if (homeWinProb > awayWinProb && homeWinProb > drawProb)
                return "Home Win";
            else if (awayWinProb > homeWinProb && awayWinProb > drawProb)
                return "Away Win";
            else
                return "Draw";
        }
    }
}
