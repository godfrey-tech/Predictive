using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapTenth : ClassMap<HistoricalMatchFromCSVFileNinth>
    {
        public MatchMapTenth()
        {
            Map(m => m.Div).Index(0);
            Map(m => m.Date).Index(1).TypeConverterOption.Format("dd/MM/yyyy"); // Adjust as necessary
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
            Map(m => m.BFH).Index(29); // Betfair Home Odds
            Map(m => m.BFD).Index(30); // Betfair Draw Odds
            Map(m => m.BFA).Index(31); // Betfair Away Odds
            Map(m => m.PSH).Index(32);
            Map(m => m.PSD).Index(33);
            Map(m => m.PSA).Index(34);
            Map(m => m.WHH).Index(35);
            Map(m => m.WHD).Index(36);
            Map(m => m.WHA).Index(37);
            Map(m => m.OneXBH).Index(38).TypeConverter<CustomDoubleConverter>(); // 1X Bet Home Odds
            Map(m => m.OneXBD).Index(39); // 1X Bet Draw Odds
            Map(m => m.OneXBA).Index(40); // 1X Bet Away Odds
            Map(m => m.MaxH).Index(41);
            Map(m => m.MaxD).Index(42);
            Map(m => m.MaxA).Index(43);
            Map(m => m.AvgH).Index(44);
            Map(m => m.AvgD).Index(45);
            Map(m => m.AvgA).Index(46);
            Map(m => m.B365Over25).Index(47);
            Map(m => m.B365Under25).Index(48);
            Map(m => m.POver25).Index(49);
            Map(m => m.PUnder25).Index(50);
            Map(m => m.MaxOver25).Index(51);
            Map(m => m.MaxUnder25).Index(52);
            Map(m => m.AvgOver25).Index(53);
            Map(m => m.AvgUnder25).Index(54);
            Map(m => m.AHh).Index(55);
            Map(m => m.B365AHH).Index(56);
            Map(m => m.B365AHA).Index(57);
            Map(m => m.PAHH).Index(58);
            Map(m => m.PAHA).Index(59);
            Map(m => m.MaxAHH).Index(60);
            Map(m => m.MaxAHA).Index(61);
            Map(m => m.AvgAHH).Index(62);
            Map(m => m.AvgAHA).Index(63);
            Map(m => m.B365CH).Index(64);
            Map(m => m.B365CD).Index(65);
            Map(m => m.B365CA).Index(66);
            Map(m => m.BWCH).Index(67);
            Map(m => m.BWCD).Index(68);
            Map(m => m.BWCA).Index(69);
            //Map(m => m.BFCH).Index(70);
            //Map(m => m.BFCD).Index(71);
            //Map(m => m.BFCA).Index(72);
            Map(m => m.PSCH).Index(73);
            Map(m => m.PSCD).Index(74);
            Map(m => m.PSCA).Index(75);
            Map(m => m.WHCH).Index(76);
            Map(m => m.WHCD).Index(77);
            Map(m => m.WHCA).Index(78);
            //Map(m => m.OneXCH).Index(79); // 1X Bet Correct Score Home Odds
            //Map(m => m.OneXCD).Index(80); // 1X Bet Correct Score Draw Odds
            //Map(m => m.OneXCA).Index(81); // 1X Bet Correct Score Away Odds
            Map(m => m.MaxCH).Index(82);
            Map(m => m.MaxCD).Index(83);
            Map(m => m.MaxCA).Index(84);
            Map(m => m.AvgCH).Index(85);
            Map(m => m.AvgCD).Index(86);
            Map(m => m.AvgCA).Index(87);
            //Map(m => m.BFECH).Index(88); // Betfair Correct Score Over 2.5 Odds
            //Map(m => m.BFECD).Index(89); // Betfair Correct Score Under 2.5 Odds
            //Map(m => m.BFECA).Index(90); // Betfair Correct Score
            Map(m => m.B365COver25).Index(91);
            Map(m => m.B365CUnder25).Index(92);

        }
    }
}
