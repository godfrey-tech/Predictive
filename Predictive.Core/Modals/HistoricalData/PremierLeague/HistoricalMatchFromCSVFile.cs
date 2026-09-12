using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFile
    {
        public string? Div { get; set; }
        public DateTime Date { get; set; }
        public string? Time { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public int? FTHG { get; set; } // Full Time Home Goals
        public int? FTAG { get; set; } // Full Time Away Goals
        public string? FTR { get; set; } // Full Time Result (A, D, H)
        public double? HTHG { get; set; } // Half Time Home Goals
        public double? HTAG { get; set; } // Half Time Away Goals
        public string? HTR { get; set; } // Half Time Result (A, D, H)
        public string? Referee { get; set; }
        public int? HS { get; set; } // Home Shots
        public int? AS { get; set; } // Away Shots
        public int? HST { get; set; } // Home Shots on Target
        public int? AST { get; set; } // Away Shots on Target
        public int? HF { get; set; } // Home Fouls
        public int? AF { get; set; } // Away Fouls
        public int? HC { get; set; } // Home Corners
        public int? AC { get; set; } // Away Corners
        public int? HY { get; set; } // Home Yellow Cards
        public int? AY { get; set; } // Away Yellow Cards
        public int? HR { get; set; } // Home Red Cards
        public int? AR { get; set; } // Away Red Cards
        public double? B365H { get; set; } // Bet365 Home Odds
        public double? B365D { get; set; } // Bet365 Draw Odds
        public double? B365A { get; set; } // Bet365 Away Odds
        public double? BWH { get; set; } // Betfair Home Odds
        public double? BWD { get; set; } // Betfair Draw Odds
        public double? BWA { get; set; } // Betfair Away Odds
        public double? IWH { get; set; } // Interwetten Home Odds
        public double? IWD { get; set; } // Interwetten Draw Odds
        public double? IWA { get; set; } // Interwetten Away Odds
        public double? PSH { get; set; } // Pinnacle Sports Home Odds
        public double? PSD { get; set; } // Pinnacle Sports Draw Odds
        public double? PSA { get; set; } // Pinnacle Sports Away Odds
        public double? WHH { get; set; } // William Hill Home Odds
        public double? WHD { get; set; } // William Hill Draw Odds
        public double? WHA { get; set; } // William Hill Away Odds
        public double? VCH { get; set; } // VC Bet Home Odds
        public double? VCD { get; set; } // VC Bet Draw Odds
        public double? VCA { get; set; } // VC Bet Away Odds
        public double? MaxH { get; set; } // Max Home Odds
        public double? MaxD { get; set; } // Max Draw Odds
        public double? MaxA { get; set; } // Max Away Odds
        public double? AvgH { get; set; } // Avg Home Odds
        public double? AvgD { get; set; } // Avg Draw Odds
        public double? AvgA { get; set; } // Avg Away Odds
        public double? B365Over25 { get; set; } // Bet365 Over 2.5 Goals Odds
        public double? B365Under25 { get; set; } // Bet365 Under 2.5 Goals Odds
        public double? POver25 { get; set; } // Probability Over 2.5 Goals
        public double? PUnder25 { get; set; } // Probability Under 2.5 Goals
        public double? MaxOver25 { get; set; } // Max Over 2.5 Goals Odds
        public double? MaxUnder25 { get; set; } // Max Under 2.5 Goals Odds
        public double? AvgOver25 { get; set; } // Avg Over 2.5 Goals Odds
        public double? AvgUnder25 { get; set; } // Avg Under 2.5 Goals Odds
        public double? AHh { get; set; } // Asian Handicap Home
        public double? B365AHH { get; set; } // Bet365 Asian Handicap Home Odds
        public double? B365AHA { get; set; } // Bet365 Asian Handicap Away Odds
        public double? PAHH { get; set; } // Probability Asian Handicap Home
        public double? PAHA { get; set; } // Probability Asian Handicap Away
        public double? MaxAHH { get; set; } // Max Asian Handicap Home Odds
        public double? MaxAHA { get; set; } // Max Asian Handicap Away Odds
        public double? AvgAHH { get; set; } // Avg Asian Handicap Home Odds
        public double? AvgAHA { get; set; } // Avg Asian Handicap Away Odds
        public double? B365CH { get; set; } // Bet365 Correct Score Home Odds
        public double? B365CD { get; set; } // Bet365 Correct Score Draw Odds
        public double? B365CA { get; set; } // Bet365 Correct Score Away Odds
        public double? BWCH { get; set; } // Betfair Correct Score Home Odds
        public double? BWCD { get; set; } // Betfair Correct Score Draw Odds
        public double? BWCA { get; set; } // Betfair Correct Score Away Odds
        public double? IWCH { get; set; } // Interwetten Correct Score Home Odds
        public double? IWCD { get; set; } // Interwetten Correct Score Draw Odds
        public double? IWCA { get; set; } // Interwetten Correct Score Away Odds
        public double? PSCH { get; set; } // Pinnacle Sports Correct Score Home Odds
        public double? PSCD { get; set; } // Pinnacle Sports Correct Score Draw Odds
        public double? PSCA { get; set; } // Pinnacle Sports Correct Score Away Odds
        public double? WHCH { get; set; } // William Hill Correct Score Home Odds
        public double? WHCD { get; set; } // William Hill Correct Score Draw Odds
        public double? WHCA { get; set; } // William Hill Correct Score Away Odds
        public double? VCCH { get; set; } // VC Bet Correct Score Home Odds
        public double? VCCD { get; set; } // VC Bet Correct Score Draw Odds
        public double? VCCA { get; set; } // VC Bet Correct Score Away Odds
        public double? MaxCH { get; set; } // Max Correct Score Home Odds
        public double? MaxCD { get; set; } // Max Correct Score Draw Odds
        public double? MaxCA { get; set; } // Max Correct Score Away Odds
        public double? AvgCH { get; set; } // Avg Correct Score Home Odds
        public double? AvgCD { get; set; } // Avg Correct Score Draw Odds
        public double? AvgCA { get; set; } // Avg Correct Score Away Odds
        public double? B365COver25 { get; set; } // Bet365 Correct Score Over 2.5 Goals Odds
        public double? B365CUnder25 { get; set; } // Bet365 Correct Score Under 2.5 Goals Odds
        public double? PCOver25 { get; set; } // Probability Correct Score Over 2.5 Goals
        public double? PCUnder25 { get; set; } // Probability Correct Score Under 2.5 Goals
        public double? MaxCOver25 { get; set; } // Max Correct Score Over 2.5 Goals Odds
        public double? MaxCUnder25 { get; set; } // Max Correct Score Under 2.5 Goals Odds
        public double? AvgCOver25 { get; set; } // Avg Correct Score Over 2.5 Goals Odds
        public double? AvgCUnder25 { get; set; } // Avg Correct Score Under 2.5 Goals Odds
        public double? AHCh { get; set; } // Asian Handicap Correct Score Home
        public double? B365CAHH { get; set; } // Bet365 Correct Score Asian Handicap Home Odds
        public double? B365CAHA { get; set; } // Bet365 Correct Score Asian Handicap Away Odds
        public double? PCAHH { get; set; } // Probability Correct Score Asian Handicap Home
        public double? PCAHA { get; set; } // Probability Correct Score Asian Handicap Away
        public double? MaxCAHH { get; set; } // Max Correct Score Asian Handicap Home Odds
        public double? MaxCAHA { get; set; } // Max Correct Score Asian Handicap Away Odds
        public double? AvgCAHH { get; set; } // Avg Correct Score Asian Handicap Home Odds
        public double? AvgCAHA { get; set; } // Avg Correct Score Asian Handicap Away Odds

    }

}
