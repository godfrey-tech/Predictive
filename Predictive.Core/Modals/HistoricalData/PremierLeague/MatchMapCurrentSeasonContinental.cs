using CsvHelper.Configuration;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    // Same 2026-27 schema change as MatchMapCurrentSeasonEnglish, but continental
    // leagues' CSVs (Bundesliga/Ligue 1/Serie A/La Liga) never carried a Referee
    // column at all — 113 columns instead of 114. Kept as a separate ClassMap
    // (rather than making Referee optional in one shared map) because CsvHelper
    // throws a HeaderValidationException for a mapped column with no matching
    // header, not just a silently-missing value.
    public class MatchMapCurrentSeasonContinental : ClassMap<HistoricalMatchFromCSVFile>
    {
        public MatchMapCurrentSeasonContinental()
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
