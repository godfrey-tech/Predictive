using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class PredictOutcome
    {
        public double HomeWinProbability { get; set; }
        public double AwayWinProbability { get; set; }
        public double DrawProbability { get; set; }
        public double DifferenceProbability { get; set; }
        public double HomeAverageGoalsScored { get; set; }
        public double AwayAverageGoalsScored { get; set; }
        public double HomeHistoricalAverageGoalsScored { get; set; }
        public double AwayHistoricalAverageGoalsScored { get; set; }
        public double AverageGoalsConceded { get; set; }
        public double Over1GoalsProbability { get; set; }
        public double Over2GoalsProbability { get; set; }
        public string Category { get; set; }
        public string Outcome { get; set; }
    }
}
