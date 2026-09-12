using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapNinth : ClassMap<HistoricalMatchFromCSVFileEighth>
    {
        public MatchMapNinth()
        {
            Map(m => m.Div).Index(0);
            Map(m => m.Date).Index(1).TypeConverterOption.Format("dd/MM/yyyy"); // Adjusted to handle date formats
            Map(m => m.Time).Index(2);
            Map(m => m.HomeTeam).Index(3);
            Map(m => m.AwayTeam).Index(4);
            Map(m => m.FTHG).Index(5);
            Map(m => m.FTAG).Index(6);
            Map(m => m.FTR).Index(7);
            Map(m => m.HTHG).Index(8);
            Map(m => m.HTAG).Index(9);
            Map(m => m.HTR).Index(10);
            Map(m => m.HS).Index(11);
            Map(m => m.AS).Index(12);
            Map(m => m.HST).Index(13);
            Map(m => m.AST).Index(14);
            Map(m => m.HF).Index(15);
            Map(m => m.AF).Index(16);
            Map(m => m.HC).Index(17);
            Map(m => m.AC).Index(18);
            Map(m => m.HY).Index(19);
            Map(m => m.AY).Index(20);
            Map(m => m.HR).Index(21);
            Map(m => m.AR).Index(22);
            Map(m => m.B365H).Index(23);
            Map(m => m.B365D).Index(24);
            Map(m => m.B365A).Index(25);
            Map(m => m.BWH).Index(26);
            Map(m => m.BWD).Index(27);
            Map(m => m.BWA).Index(28);
            Map(m => m.IWH).Index(29);
            Map(m => m.IWD).Index(30);
            Map(m => m.IWA).Index(31);
            Map(m => m.LBH).Index(32);
            Map(m => m.LBD).Index(33);
            Map(m => m.LBA).Index(34);
            Map(m => m.PSH).Index(35);
            Map(m => m.PSD).Index(36);
            Map(m => m.PSA).Index(37);
            Map(m => m.WHH).Index(38);
            Map(m => m.WHD).Index(39);
            Map(m => m.WHA).Index(40);
            Map(m => m.SJH).Index(41); // Optional, if present
            Map(m => m.SJD).Index(42); // Optional, if present
            Map(m => m.SJA).Index(43); // Optional, if present
            Map(m => m.VCH).Index(44);
            Map(m => m.VCD).Index(45);
            Map(m => m.VCA).Index(46);
            Map(m => m.Bb1X2).Index(47);
            Map(m => m.BbMxH).Index(48);
            Map(m => m.BbAvH).Index(49);
            Map(m => m.BbMxD).Index(50);
            Map(m => m.BbAvD).Index(51);
            Map(m => m.BbMxA).Index(52);
            Map(m => m.BbAvA).Index(53);
            Map(m => m.BbOU).Index(54);
            Map(m => m.BbMxOver25).Index(55);
            Map(m => m.BbAvOver25).Index(56);
            Map(m => m.BbMxUnder25).Index(57);
            Map(m => m.BbAvUnder25).Index(58);
            Map(m => m.BbAH).Index(59);
            Map(m => m.BbAHh).Index(60);
            Map(m => m.BbMxAHH).Index(61);
            Map(m => m.BbAvAHH).Index(62);
            Map(m => m.BbMxAHA).Index(63);
            Map(m => m.BbAvAHA).Index(64).TypeConverter<CustomDoubleConverter>();
            Map(m => m.PSCH).Index(65);
            Map(m => m.PSCD).Index(66);
            Map(m => m.PSCA).Index(67);
            //Map(m => m.WHCH).Index(68);
            //Map(m => m.WHCD).Index(69);
            //Map(m => m.WHCA).Index(70);
            //Map(m => m.VCCH).Index(71);
            //Map(m => m.VCCD).Index(72);
            //Map(m => m.VCCA).Index(73);
            //Map(m => m.MaxCH).Index(74);
            //Map(m => m.MaxCD).Index(75);
            //Map(m => m.MaxCA).Index(76);
            //Map(m => m.AvgCH).Index(77);
            //Map(m => m.AvgCD).Index(78);
            //Map(m => m.AvgCA).Index(79);
            //Map(m => m.B365COver25).Index(80);
            //Map(m => m.B365CUnder25).Index(81);
            //Map(m => m.PCOver25).Index(82);
            //Map(m => m.PCUnder25).Index(83);
            //Map(m => m.MaxCOver25).Index(84);
            //Map(m => m.MaxCUnder25).Index(85);
            //Map(m => m.AvgCOver25).Index(86);
            //Map(m => m.AvgCUnder25).Index(87);
            //Map(m => m.AHCh).Index(88);
            //Map(m => m.B365CAHH).Index(89);
            //Map(m => m.B365CAHA).Index(90);
            //Map(m => m.PCAHH).Index(91);
            //Map(m => m.PCAHA).Index(92);

        }
    }
}
