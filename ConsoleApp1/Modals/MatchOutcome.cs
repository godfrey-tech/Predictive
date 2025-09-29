using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchOutcome
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int FTHG { get; set; } // Full Time Home Goals
        public int FTAG { get; set; } // Full Time Away Goals
        public string FTR { get; set; } // Full Time Result (H, D, A)
    }
}
