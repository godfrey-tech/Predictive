using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapSeventh : ClassMap<HistoricalMatchFromCSVFileSeventh>
    {
        public MatchMapSeventh()
        {
            Map(m => m.Div);
            Map(m => m.Date).TypeConverter<CustomDateConverter>(); //.TypeConverterOption.Format("dd/MM/yy"); ;
            Map(m => m.HomeTeam);
            Map(m => m.AwayTeam);
            Map(m => m.FTHG);
            Map(m => m.FTAG);
            Map(m => m.FTR);
            Map(m => m.HTHG);
            Map(m => m.HTAG);
            Map(m => m.HTR);
            Map(m => m.B365H);
            Map(m => m.B365D);
            Map(m => m.B365A);
            Map(m => m.BWH);
            Map(m => m.BWD);
            Map(m => m.BWA);
            Map(m => m.IWH);
            Map(m => m.IWD);
            Map(m => m.IWA);
            Map(m => m.LBH);
            Map(m => m.LBD);
            Map(m => m.LBA);
            Map(m => m.PSH);
            Map(m => m.PSD);
            Map(m => m.PSA);
            Map(m => m.WHH);
            Map(m => m.WHD);
            Map(m => m.WHA);
            //Map(m => m.SJH);
            //Map(m => m.SJD);
            //Map(m => m.SJA);
            Map(m => m.VCH);
            Map(m => m.VCD);
            Map(m => m.VCA);
            Map(m => m.Bb1X2);
            Map(m => m.BbMxH);
            Map(m => m.BbAvH);
            Map(m => m.BbMxD);
            Map(m => m.BbAvD);
            Map(m => m.BbMxA);
            Map(m => m.BbAvA);
            Map(m => m.BbOU);
            Map(m => m.BbMxOver25).Name("BbMx>2.5");
            Map(m => m.BbAvOver25).Name("BbAv>2.5");
            Map(m => m.BbMxUnder25).Name("BbMx<2.5");
            Map(m => m.BbAvUnder25).Name("BbAv<2.5");
            Map(m => m.BbAH);
            Map(m => m.BbAHh);
            Map(m => m.BbMxAHH);
            Map(m => m.BbAvAHH);
            Map(m => m.BbMxAHA);
            Map(m => m.BbAvAHA);
            Map(m => m.PSCH);
            Map(m => m.PSCD);
            Map(m => m.PSCA);

        }
    }
}
