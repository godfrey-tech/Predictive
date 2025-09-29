using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchSummary
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int TotalCorners { get; set; }
        public int TotalBookings { get; set; }
    }
}
