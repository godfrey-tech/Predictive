using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchData
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public double HomeGoalsAvg { get; set; }
        public double AwayGoalsAvg { get; set; }
        public double HomeGoalsConcededAvg { get; set; }
        public double AwayGoalsConcededAvg { get; set; }
        public double HomeScore { get; set; }
        public double AwayScore { get; set; }
        public string Outcome { get; set; } // e.g., "HomeWin", "AwayWin", "Draw"

        // New features based on recent matches
        public int HomeWinsLast5 { get; set; }
        public int HomeLossesLast5 { get; set; }
        public int HomeDrawsLast5 { get; set; }
        public double HomeAverageGoalsScoredLast5 { get; set; }
        public double HomeAverageGoalsConcededLast5 { get; set; }

        public int AwayWinsLast5 { get; set; }
        public int AwayLossesLast5 { get; set; }
        public int AwayDrawsLast5 { get; set; }
        public double AwayAverageGoalsScoredLast5 { get; set; }
        public double AwayAverageGoalsConcededLast5 { get; set; }
    }
}
