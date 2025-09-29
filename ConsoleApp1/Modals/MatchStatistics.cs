using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchStatistics
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        // New properties for halftime statistics
        public int HalfTimeWins { get; set; }
        public int HalfTimeLosses { get; set; }
        public int HalfTimeDraws { get; set; }

        // Total goals scored
        public int TotalGoalsScored { get; set; }
        public int TotalHomeGoalsScored { get; set; }
        public int TotalAwayGoalsScored { get; set; }

    }
}
