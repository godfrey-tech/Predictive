using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFileSixth
    {
        public string Div { get; set; }
        public DateTime? Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? FTHG { get; set; } // Full Time Home Goals
        public int? FTAG { get; set; } // Full Time Away Goals
        public string? FTR { get; set; } // Full Time Result
        public int? HTHG { get; set; } // Half Time Home Goals
        public int? HTAG { get; set; } // Half Time Away Goals
        public string HTR { get; set; } // Half Time Result
        public decimal? B365H { get; set; } // Bet365 Home Odds
        public decimal? B365D { get; set; } // Bet365 Draw Odds
        public decimal? B365A { get; set; } // Bet365 Away Odds
        public decimal? BWH { get; set; } // Betfair Home Odds
        public decimal? BWD { get; set; } // Betfair Draw Odds
        public decimal? BWA { get; set; } // Betfair Away Odds
        public decimal? IWH { get; set; } // Interwetten Home Odds
        public decimal? IWD { get; set; } // Interwetten Draw Odds
        public decimal? IWA { get; set; } // Interwetten Away Odds
        public decimal? LBH { get; set; } // Ladbrokes Home Odds
        public decimal? LBD { get; set; } // Ladbrokes Draw Odds
        public decimal? LBA { get; set; } // Ladbrokes Away Odds
        public decimal? PSH { get; set; } // Pinnacle Sports Home Odds
        public decimal? PSD { get; set; } // Pinnacle Sports Draw Odds
        public decimal? PSA { get; set; } // Pinnacle Sports Away Odds
        public decimal? WHH { get; set; } // William Hill Home Odds
        public decimal? WHD { get; set; } // William Hill Draw Odds
        public decimal? WHA { get; set; } // William Hill Away Odds
        public decimal? SJH { get; set; } // Sporting Index Home Odds
        public decimal? SJD { get; set; } // Sporting Index Draw Odds
        public decimal? SJA { get; set; } // Sporting Index Away Odds
        public decimal? VCH { get; set; } // VC Bet Home Odds
        public decimal? VCD { get; set; } // VC Bet Draw Odds
        public decimal? VCA { get; set; } // VC Bet Away Odds
        public decimal? Bb1X2 { get; set; } // 1X2 Odds
        public decimal? BbMxH { get; set; } // Max Home Odds
        public decimal? BbAvH { get; set; } // Avg Home Odds
        public decimal? BbMxD { get; set; } // Max Draw Odds
        public decimal? BbAvD { get; set; } // Avg Draw Odds
        public decimal? BbMxA { get; set; } // Max Away Odds
        public decimal? BbAvA { get; set; } // Avg Away Odds
        public decimal? BbOU { get; set; } // Over/Under Odds
        public decimal? BbMxOver2_5 { get; set; } // Max Odds for over 2.5 goals
        public decimal? BbAvOver2_5 { get; set; } // Avg Odds for over 2.5 goals
        public decimal? BbMxUnder2_5 { get; set; } // Max Odds for under 2.5 goals
        public decimal? BbAvUnder2_5 { get; set; }
        public decimal? BbAH { get; set; } // Asian Handicap Odds
        public decimal? BbAHh { get; set; } // Asian Handicap Half Odds
        public decimal? BbMxAHH { get; set; } // Max Asian Handicap Home Odds
        public decimal? BbAvAHH { get; set; } // Avg Asian Handicap Home Odds
        public decimal? BbMxAHA { get; set; } // Max Asian Handicap Away Odds
        public decimal? BbAvAHA { get; set; } // Avg Asian Handicap Away Odds
        public decimal? PSCH { get; set; } // Pinnacle Sports Home Odds
        public decimal? PSCD { get; set; } // Pinnacle Sports Draw Odds
        public decimal? PSCA { get; set; } // Pinnacle Sports Away Odds

    }
}
