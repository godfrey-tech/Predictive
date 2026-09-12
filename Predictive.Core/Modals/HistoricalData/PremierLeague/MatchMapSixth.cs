using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapSixth : ClassMap<HistoricalMatchFromCSVFileSixth>
    {
        public MatchMapSixth()
        {
            Map(x => x.Div);
            Map(x => x.Date).TypeConverterOption.Format("dd/MM/yy");
            Map(x => x.HomeTeam);
            Map(x => x.AwayTeam);
            Map(x => x.FTHG); // Full Time Home Goals
            Map(x => x.FTAG); // Full Time Away Goals
            Map(x => x.FTR); // Full Time Result
            Map(x => x.HTHG); // Half Time Home Goals
            Map(x => x.HTAG); // Half Time Away Goals
            Map(x => x.HTR); // Half Time Result
            Map(x => x.B365H); // Bet365 Home Odds
            Map(x => x.B365D); // Bet365 Draw Odds
            Map(x => x.B365A); // Bet365 Away Odds
            Map(x => x.BWH); // Betfair Home Odds
            Map(x => x.BWD); // Betfair Draw Odds
            Map(x => x.BWA); // Betfair Away Odds
            Map(x => x.IWH); // Interwetten Home Odds
            Map(x => x.IWD); // Interwetten Draw Odds
            Map(x => x.IWA); // Interwetten Away Odds
            Map(x => x.LBH); // Ladbrokes Home Odds
            Map(x => x.LBD); // Ladbrokes Draw Odds
            Map(x => x.LBA); // Ladbrokes Away Odds
            Map(x => x.PSH); // Pinnacle Sports Home Odds
            Map(x => x.PSD); // Pinnacle Sports Draw Odds
            Map(x => x.PSA); // Pinnacle Sports Away Odds
            Map(x => x.WHH); // William Hill Home Odds
            Map(x => x.WHD); // William Hill Draw Odds
            Map(x => x.WHA); // William Hill Away Odds
            Map(x => x.SJH); // Sporting Index Home Odds
            Map(x => x.SJD); // Sporting Index Draw Odds
            Map(x => x.SJA); // Sporting Index Away Odds
            Map(x => x.VCH); // VC Bet Home Odds
            Map(x => x.VCD); // VC Bet Draw Odds
            Map(x => x.VCA); // VC Bet Away Odds
            Map(x => x.Bb1X2); // 1X2 Odds
            Map(x => x.BbMxH); // Max Home Odds
            Map(x => x.BbAvH); // Avg Home Odds
            Map(x => x.BbMxD); // Max Draw Odds
            Map(x => x.BbAvD); // Avg Draw Odds
            Map(x => x.BbMxA); // Max Away Odds
            Map(x => x.BbAvA); // Avg Away Odds
            Map(x => x.BbOU); // Over/Under Odds
            Map(m => m.BbMxOver2_5).Name("BbMx>2.5");
            Map(m => m.BbAvOver2_5).Name("BbAv>2.5");
            Map(m => m.BbMxUnder2_5).Name("BbMx<2.5");
            Map(m => m.BbAvUnder2_5).Name("BbAv<2.5");
            Map(x => x.BbAH); // Asian Handicap Odds
            Map(x => x.BbAHh); // Asian Handicap Half Odds
            Map(x => x.BbMxAHH); // Max Asian Handicap Home Odds
            Map(x => x.BbAvAHH); // Avg Asian Handicap Home Odds
            Map(x => x.BbMxAHA); // Max Asian Handicap Away Odds
            Map(x => x.BbAvAHA); // Avg Asian Handicap Away Odds
            Map(x => x.PSCH).TypeConverter<CustomDecimalConverter>(); // Pinnacle Sports Home Odds
            Map(x => x.PSCD); // Pinnacle Sports Draw Odds
            Map(x => x.PSCA); // Pinnacle Sports Away Odds


          

        }

    }
}
