using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapEighth : ClassMap<HistoricalMatchFromCSVFileEighth>
    {
        public MatchMapEighth()
        {
            Map(m => m.Div).Index(0);
            Map(m => m.Date).Index(1).TypeConverter<CustomDateConverter>().Optional();
            Map(m => m.HomeTeam).Index(2).Optional();
            Map(m => m.AwayTeam).Index(3).Optional();
            Map(m => m.FTHG).Index(4).Optional();
            Map(m => m.FTAG).Index(5).Optional();
            Map(m => m.FTR).Index(6).Optional();
            Map(m => m.HTHG).Index(7).Optional();
            Map(m => m.HTAG).Index(8).Optional();
            Map(m => m.HTR).Index(9).Optional();
            Map(m => m.HS).Index(10).Optional();
            Map(m => m.AS).Index(11).Optional();
            Map(m => m.HST).Index(12).Optional();
            Map(m => m.AST).Index(13).Optional();
            Map(m => m.HF).Index(14).Optional();
            Map(m => m.AF).Index(15).Optional();
            Map(m => m.HC).Index(16).Optional();
            Map(m => m.AC).Index(17).Optional();
            Map(m => m.HY).Index(18).Optional();
            Map(m => m.AY).Index(19).Optional();
            Map(m => m.HR).Index(20).Optional();
            Map(m => m.AR).Index(21).Optional();
            Map(m => m.B365H).Index(22).Optional();
            Map(m => m.B365D).Index(23).Optional();
            Map(m => m.B365A).Index(24).Optional();
            Map(m => m.BWH).Index(25).Optional();
            Map(m => m.BWD).Index(26).Optional();
            Map(m => m.BWA).Index(27).Optional();
            Map(m => m.IWH).Index(28).Optional();
            Map(m => m.IWD).Index(29).Optional();
            Map(m => m.IWA).Index(30).Optional();
            Map(m => m.LBH).Index(31).Optional();
            Map(m => m.LBD).Index(32).Optional();
            Map(m => m.LBA).Index(33).Optional();
            Map(m => m.PSH).Index(34).Optional();
            Map(m => m.PSD).Index(35).Optional();
            Map(m => m.PSA).Index(36).Optional();
            Map(m => m.WHH).Index(37).Optional();
            Map(m => m.WHD).Index(38).Optional();
            Map(m => m.WHA).Index(39).Optional();
            Map(m => m.SJH).Index(40).Optional();
            Map(m => m.SJD).Index(41).Optional();
            Map(m => m.SJA).Index(42).Optional();
            Map(m => m.VCH).Index(43).Optional();
            Map(m => m.VCD).Index(44).Optional();
            Map(m => m.VCA).Index(45).Optional();
            Map(m => m.Bb1X2).Index(46).Optional();
            Map(m => m.BbMxH).Index(47).Optional();
            Map(m => m.BbAvH).Index(48).Optional();
            Map(m => m.BbMxD).Index(49).Optional();
            Map(m => m.BbAvD).Index(50).Optional();
            Map(m => m.BbMxA).Index(51).Optional();
            Map(m => m.BbAvA).Index(52).Optional();
            Map(m => m.BbOU).Index(53).Optional();
            Map(m => m.BbMxOver25).Index(54).Optional();
            Map(m => m.BbAvOver25).Index(55).Optional();
            Map(m => m.BbMxUnder25).Index(56).Optional();
            Map(m => m.BbAvUnder25).Index(57).Optional();
            Map(m => m.BbAH).Index(58).Optional();
            Map(m => m.BbAHh).Index(59).Optional();
            Map(m => m.BbMxAHH).Index(60).Optional();
            Map(m => m.BbAvAHH).Index(61).Optional();
            Map(m => m.BbMxAHA).Index(62).Optional();
            Map(m => m.BbAvAHA).Index(63).Optional();
            Map(m => m.PSCH).Index(64).Optional();
            Map(m => m.PSCD).Index(65).Optional();
            Map(m => m.PSCA).Index(66).Optional();
        }

    }
}
