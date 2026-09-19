using CsvHelper.Configuration;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    // football-data.co.uk changed its schema for the 2026-27 season: added HxG/AxG
    // (expected goals) and dropped several bookmakers (Interwetten, Pinnacle, William
    // Hill, VC Bet) — 114 columns for English leagues (E0/E1, which carry Referee),
    // 113 for continental ones (see MatchMapCurrentSeasonContinental). Neither count
    // matched any existing variant, so the current season was silently skipped
    // entirely (discovered 2026-09-19). Only maps columns confirmed present in the
    // new header — B365/Max/Avg still exist, Interwetten/Pinnacle/WH/VC don't.
    public class MatchMapCurrentSeasonEnglish : ClassMap<HistoricalMatchFromCSVFile>
    {
        public MatchMapCurrentSeasonEnglish()
        {
            Map(m => m.Div);
            Map(m => m.Date).TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Time);
            Map(m => m.HomeTeam);
            Map(m => m.AwayTeam);
            Map(m => m.FTHG);
            Map(m => m.FTAG);
            Map(m => m.FTR);
            Map(m => m.HTHG);
            Map(m => m.HTAG);
            Map(m => m.HTR);
            Map(m => m.Referee);
            Map(m => m.HS);
            Map(m => m.AS);
            Map(m => m.HST);
            Map(m => m.AST);
            Map(m => m.HF);
            Map(m => m.AF);
            Map(m => m.HC);
            Map(m => m.AC);
            Map(m => m.HY);
            Map(m => m.AY);
            Map(m => m.HR);
            Map(m => m.AR);
            Map(m => m.B365H);
            Map(m => m.B365D);
            Map(m => m.B365A);
            Map(m => m.MaxH);
            Map(m => m.MaxD);
            Map(m => m.MaxA);
            Map(m => m.AvgH);
            Map(m => m.AvgD);
            Map(m => m.AvgA);
            Map(m => m.B365Over25).Name("B365>2.5");
            Map(m => m.B365Under25).Name("B365<2.5");
            Map(m => m.MaxOver25).Name("Max>2.5");
            Map(m => m.MaxUnder25).Name("Max<2.5");
            Map(m => m.AvgOver25).Name("Avg>2.5");
            Map(m => m.AvgUnder25).Name("Avg<2.5");
        }
    }
}
