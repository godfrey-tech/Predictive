using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchData
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int HomeGoalsConceded { get; set; }
        public int AwayGoalsConceded { get; set; }
        public string Outcome { get; set; } // e.g., "HomeWin", "AwayWin", "Draw"
        public DateTime MatchDate { get; set; } // New property for the match date
        public string Competition { get; set; }
        public int? HomeHalfTimeScore { get; set; } // Nullable to handle matches that may not have this data
        public int? AwayHalfTimeScore { get; set; }
    }

}
