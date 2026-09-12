using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFileFourth
    {
        public string Div { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int FTHG { get; set; } // Full Time Home Goals
        public int FTAG { get; set; } // Full Time Away Goals
        public string FTR { get; set; } // Full Time Result
        public double? HTHG { get; set; } // Half Time Home Goals
        public double? HTAG { get; set; } // Half Time Away Goals
        public string HTR { get; set; } // Half Time Result
        public string Referee { get; set; }
        public double? HS { get; set; } // Home Shots
        public double? AS { get; set; } // Away Shots
        public double? HST { get; set; } // Home Shots on Target
        public double? AST { get; set; } // Away Shots on Target
        public double? HF { get; set; } // Home Fouls
        public double? AF { get; set; } // Away Fouls
        public double? HC { get; set; } // Home Corners
        public double? AC { get; set; } // Away Corners
        public double? HY { get; set; } // Home Yellow Cards
        public double? AY { get; set; } // Away Yellow Cards
        public double? HR { get; set; } // Home Red Cards
        public double? AR { get; set; } // Away Red Cards
        public decimal B365H { get; set; } // Bet365 Home Odds
        public decimal B365D { get; set; } // Bet365 Draw Odds
        public decimal B365A { get; set; } // Bet365 Away Odds
        public decimal? BWH { get; set; } // Betfair Home Odds
        public decimal? BWD { get; set; } // Betfair Draw Odds
        public decimal? BWA { get; set; } // Betfair Away Odds
        public decimal? IWH { get; set; } // Interwetten Home Odds
        public decimal? IWD { get; set; } // Interwetten Draw Odds
        public decimal? IWA { get; set; } // Interwetten Away Odds
        public decimal? PSH { get; set; } // Pinnacle Sports Home Odds
        public decimal? PSD { get; set; } // Pinnacle Sports Draw Odds
        public decimal? PSA { get; set; } // Pinnacle Sports Away Odds
        public decimal? WHH { get; set; } // William Hill Home Odds
        public decimal? WHD { get; set; } // William Hill Draw Odds
        public decimal? WHA { get; set; } // William Hill Away Odds
        public decimal? VCH { get; set; } // VC Bet Home Odds
        public decimal? VCD { get; set; } // VC Bet Draw Odds
        public decimal? VCA { get; set; } // VC Bet Away Odds
        public string  Bb1X2 { get; set; } // Betbrain 1X2
        public decimal BbMxH { get; set; } // Betbrain Max Home Odds
        public decimal BbAvH { get; set; } // Betbrain Avg Home Odds
        public decimal BbMxD { get; set; } // Betbrain Max Draw Odds
        public decimal BbAvD { get; set; } // Betbrain Avg Draw Odds
        public decimal BbMxA { get; set; } // Betbrain Max Away Odds
        public decimal BbAvA { get; set; } // Betbrain Avg Away Odds
        public string BbOU { get; set; } // Betbrain Over/Under
        public decimal BbMxGreaterThan2_5 { get; set; } // Betbrain Max Odds >2.5
        public decimal BbAvGreaterThan2_5 { get; set; } // Betbrain Avg Odds >2.5
        public decimal BbMxLessThan2_5 { get; set; } // Betbrain Max Odds <2.5
        public decimal BbAvLessThan2_5 { get; set; } // Betbrain Avg Odds <2.5
        public string BbAH { get; set; } // Betbrain Asian Handicap
        public string BbAHh { get; set; } // Betbrain Asian Handicap Half
        public decimal BbMxAHH { get; set; } // Betbrain Max Asian Handicap Home
        public decimal BbAvAHH { get; set; } // Betbrain Avg Asian Handicap Home
        public decimal BbMxAHA { get; set; } // Betbrain Max Asian Handicap Away
        public decimal BbAvAHA { get; set; } // Betbrain Avg Asian Handicap Away
        public decimal? PSCH { get; set; } // Pinnacle Sports Home Odds
        public decimal? PSCD { get; set; } // Pinnacle Sports Draw Odds
        public decimal? PSCA { get; set; } // Pinnacle Sports Away Odds

    }

}
