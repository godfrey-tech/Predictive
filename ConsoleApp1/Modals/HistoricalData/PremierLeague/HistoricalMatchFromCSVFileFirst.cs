using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFileFirst
    {
        public string? Div { get; set; }            // Division
        public DateTime? Date { get; set; }         // Match Date
        public string? HomeTeam { get; set; }       // Home Team
        public string? AwayTeam { get; set; }       // Away Team
        public int? FTHG { get; set; }              // Full Time Home Goals
        public int? FTAG { get; set; }              // Full Time Away Goals
        public string? FTR { get; set; }            // Full Time Result (A, D, H)
        public int? HTHG { get; set; }              // Half Time Home Goals
        public int? HTAG { get; set; }              // Half Time Away Goals
        public string? HTR { get; set; }            // Half Time Result (A, D, H)
        public string? Referee { get; set; }        // Referee Name
        public int? HS { get; set; }                // Home Shots
        public int? AS { get; set; }                // Away Shots
        public int? HST { get; set; }               // Home Shots on Target
        public int? AST { get; set; }               // Away Shots on Target
        public int? HF { get; set; }                // Home Fouls
        public int? AF { get; set; }                // Away Fouls
        public int? HC { get; set; }                // Home Corners
        public int? AC { get; set; }                // Away Corners
        public int? HY { get; set; }                // Home Yellow Cards
        public int? AY { get; set; }                // Away Yellow Cards
        public int? HR { get; set; }                // Home Red Cards
        public int? AR { get; set; }                // Away Red Cards
        public double? B365H { get; set; }          // Bet365 Home Odds
        public double? B365D { get; set; }          // Bet365 Draw Odds
        public double? B365A { get; set; }          // Bet365 Away Odds
        public double? BWH { get; set; }            // Betfair Home Odds
        public double? BWD { get; set; }            // Betfair Draw Odds
        public double? BWA { get; set; }            // Betfair Away Odds
        public double? IWH { get; set; }            // Interwetten Home Odds
        public double? IWD { get; set; }            // Interwetten Draw Odds
        public double? IWA { get; set; }            // Interwetten Away Odds
        public double? LBH { get; set; }            // Ladbrokes Home Odds
        public double? LBD { get; set; }            // Ladbrokes Draw Odds
        public double? LBA { get; set; }            // Ladbrokes Away Odds
        public double? PSH { get; set; }            // Pinnacle Sports Home Odds
        public double? PSD { get; set; }            // Pinnacle Sports Draw Odds
        public double? PSA { get; set; }            // Pinnacle Sports Away Odds
        public double? WHH { get; set; }            // William Hill Home Odds
        public double? WHD { get; set; }            // William Hill Draw Odds
        public double? WHA { get; set; }            // William Hill Away Odds
        public double? SJH { get; set; }            // Spreadex Home Odds
        public double? SJD { get; set; }            // Spreadex Draw Odds
        public double? SJA { get; set; }            // Spreadex Away Odds
        public double? VCH { get; set; }            // VC Bet Home Odds
        public double? VCD { get; set; }            // VC Bet Draw Odds
        public double? VCA { get; set; }            // VC Bet Away Odds
        public double? Bb1X2 { get; set; }          // BetBuilder 1X2 Odds
        public double? BbMxH { get; set; }          // BetBuilder Max Home Odds
        public double? BbAvH { get; set; }          // BetBuilder Avg Home Odds
        public double? BbMxD { get; set; }          // BetBuilder Max Draw Odds
        public double? BbAvD { get; set; }          // BetBuilder Avg Draw Odds
        public double? BbMxA { get; set; }          // BetBuilder Max Away Odds
        public double? BbAvA { get; set; }          // BetBuilder Avg Away Odds
        public double? BbOU { get; set; }            // BetBuilder Over/Under Odds
        public double? BbMxOver25 { get; set; }     // BetBuilder Max Over 2.5 Goals Odds
        public double? BbAvOver25 { get; set; }     // BetBuilder Avg Over 2.5 Goals Odds
        public double? BbMxUnder25 { get; set; }    // BetBuilder Max Under 2.5 Goals Odds
        public double? BbAvUnder25 { get; set; }    // BetBuilder Avg Under 2.5 Goals Odds
        public double? BbAH { get; set; }            // BetBuilder Asian Handicap Odds
        public double? BbAHh { get; set; }           // BetBuilder Asian Handicap Home Odds
        public double? BbMxAHH { get; set; }        // BetBuilder Max Asian Handicap Home Odds
        public double? BbAvAHH { get; set; }        // BetBuilder Avg Asian Handicap Home Odds
        public double? BbMxAHA { get; set; }        // BetBuilder Max Asian Handicap Away Odds
        public double? BbAvAHA { get; set; }        // BetBuilder Avg Asian Handicap Away Odds
        public double? PSCH { get; set; }            // Pinnacle Sports Correct Score Home Odds
        public double? PSCD { get; set; }            // Pinnacle Sports Correct Score Draw Odds
        public double? PSCA { get; set; }            // Pinnacle Sports Correct Score Away Odds

    }
}
