using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class HeadToHead
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int TotalMatches { get; set; }
        public int HomeWins { get; set; }
        public int AwayWins { get; set; }
        public int Draws { get; set; }
    }
}
