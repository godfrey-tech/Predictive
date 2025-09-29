using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class TeamStatistics
    {
        public int TotalWins { get; set; }
        public int TotalDraws { get; set; }
        public int TotalLosses { get; set; }
        public int TotalMatches { get; set; }
        public double AverageGoalsScored { get; set; }
        public double AverageGoalsConceded { get; set; }
        public double GoalDifference { get; set; }
        public double RecentForm { get; set; }
        public double HomeAwayPerformance { get; set; }
        public double WinStreak { get; set; }
    }
}
