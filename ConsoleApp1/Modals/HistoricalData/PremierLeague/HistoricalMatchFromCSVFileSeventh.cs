using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFileSeventh
    {
        public string Div { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? FTHG { get; set; } // Full Time Home Goals
        public int? FTAG { get; set; } // Full Time Away Goals
        public string FTR { get; set; } // Full Time Result
        public int? HTHG { get; set; } // Half Time Home Goals
        public int? HTAG { get; set; } // Half Time Away Goals
        public string? HTR { get; set; } // Half Time Result
        public double? B365H { get; set; } // Bet365 Home Odds
        public double? B365D { get; set; } // Bet365 Draw Odds
        public double? B365A { get; set; } // Bet365 Away Odds
        public double? BWH { get; set; } // BetWin Home Odds
        public double? BWD { get; set; } // BetWin Draw Odds
        public double? BWA { get; set; } // BetWin Away Odds
        public double? IWH { get; set; } // Interwetten Home Odds
        public double? IWD { get; set; } // Interwetten Draw Odds
        public double? IWA { get; set; } // Interwetten Away Odds
        public double? LBH { get; set; } // Ladbrokes Home Odds
        public double? LBD { get; set; } // Ladbrokes Draw Odds
        public double? LBA { get; set; } // Ladbrokes Away Odds
        public double? PSH { get; set; } // Pinnacle Sports Home Odds
        public double? PSD { get; set; } // Pinnacle Sports Draw Odds
        public double? PSA { get; set; } // Pinnacle Sports Away Odds
        public double? WHH { get; set; } // William Hill Home Odds
        public double? WHD { get; set; } // William Hill Draw Odds
        public double? WHA { get; set; } // William Hill Away Odds
        //public double SJH { get; set; } // Sporting Index Home Odds
        //public double SJD { get; set; } // Sporting Index Draw Odds
        //public double SJA { get; set; } // Sporting Index Away Odds
        public double? VCH { get; set; } // VC Bet Home Odds
        public double? VCD { get; set; } // VC Bet Draw Odds
        public double? VCA { get; set; } // VC Bet Away Odds
        public string? Bb1X2 { get; set; } // BetBrain 1X2
        public double? BbMxH { get; set; } // BetBrain Max Home Odds
        public double? BbAvH { get; set; } // BetBrain Avg Home Odds
        public double? BbMxD { get; set; } // BetBrain Max Draw Odds
        public double? BbAvD { get; set; } // BetBrain Avg Draw Odds
        public double? BbMxA { get; set; } // BetBrain Max Away Odds
        public double? BbAvA { get; set; } // BetBrain Avg Away Odds
        public double? BbOU { get; set; } // BetBrain Over/Under Odds
        public double? BbMxOver25 { get; set; } // BetBrain Max Over 2.5 Odds
        public double? BbAvOver25 { get; set; } // BetBrain Avg Over 2.5 Odds
        public double? BbMxUnder25 { get; set; } // BetBrain Max Under 2.5 Odds
        public double? BbAvUnder25 { get; set; } // BetBrain Avg Under 2.5 Odds
        public double? BbAH { get; set; } // BetBrain Asian Handicap Odds
        public double? BbAHh { get; set; } // BetBrain Asian Handicap Half Odds
        public double? BbMxAHH { get; set; } // BetBrain Max Asian Handicap Home Odds
        public double? BbAvAHH { get; set; } // BetBrain Avg Asian Handicap Home Odds
        public double? BbMxAHA { get; set; } // BetBrain Max Asian Handicap Away Odds
        public double? BbAvAHA { get; set; } // BetBrain Avg Asian Handicap Away Odds
        public double? PSCH { get; set; } // Pinnacle Sports Asian Home Odds
        public double? PSCD { get; set; } // Pinnacle Sports Asian Draw Odds
        public double? PSCA { get; set; } // Pinnacle Sports Asian Away Odds

    }
}
