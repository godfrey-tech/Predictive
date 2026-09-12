using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public  class SeasonMatchRecord
    {
        public string LeagueIdentifier { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public double HomeHalfTimeGoals { get; set; }
        public double AwayHalfTimeGoals { get; set; }
        public int HomeCorners { get; set; }
        public int AwayCorners { get; set; }
        public int HomeHalfTimeCorners { get; set; }
        public int AwayHalfTimeCorners { get; set; }
        public int HomeBookings { get; set; }
        public int AwayBookings { get; set; }
        public int HomeHalfTimeBookings { get; set; }
        public int AwayHalfTimeBookings { get; set; }

        // Referee and match-result/goals odds are already parsed out of the CSVs
        // upstream (see HistoricalMatchFromCSVFile) but were being dropped here.
        // Null when the source CSV variant doesn't carry them (e.g. Extra Leagues).
        public string? Referee { get; set; }
        public double? AvgHomeOdds { get; set; }
        public double? AvgDrawOdds { get; set; }
        public double? AvgAwayOdds { get; set; }
        public double? AvgOver25Odds { get; set; }
        public double? AvgUnder25Odds { get; set; }
    }
}
