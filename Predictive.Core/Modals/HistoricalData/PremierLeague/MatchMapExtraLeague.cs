using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class MatchMapExtraLeague : ClassMap<HistoricalMatchFromCSVFileExtraLeague>
    {
        public MatchMapExtraLeague()
        {
            Map(m => m.Country).Index(0);
            Map(m => m.League).Index(1);
            Map(m => m.Season).Index(2);
            Map(m => m.Date).Index(3).TypeConverter<CustomDateConverter>(); //TypeConverterOption.Format("dd/MM/yyyy"); // Adjust format as needed
            Map(m => m.Time).Index(4);
            Map(m => m.Home).Index(5);
            Map(m => m.Away).Index(6);
            Map(m => m.HG).Index(7); // Home Goals
            Map(m => m.AG).Index(8); // Away Goals
            Map(m => m.Res).Index(9); // Result
            Map(m => m.PSCH).Index(10); // Pinnacle Sports Correct Score Home Odds
            Map(m => m.PSCD).Index(11); // Pinnacle Sports Correct Score Draw Odds
            Map(m => m.PSCA).Index(12); // Pinnacle Sports Correct Score Away Odds
            Map(m => m.MaxCH).Index(13); // Max Correct Score Home Odds
            Map(m => m.MaxCD).Index(14); // Max Correct Score Draw Odds
            Map(m => m.MaxCA).Index(15); // Max Correct Score Away Odds
            Map(m => m.AvgCH).Index(16).TypeConverter<CustomDoubleConverter>(); // Avg Correct Score Home Odds
            Map(m => m.AvgCD).Index(17); // Avg Correct Score Draw Odds
            Map(m => m.AvgCA).Index(18); // Avg Correct Score Away Odds
            Map(m => m.BFECH).Index(19); // Betfair Correct Score Home Odds
            Map(m => m.BFECD).Index(20); // Betfair Correct Score Draw Odds
            Map(m => m.BFECA).Index(21); // Betfair Correct Score Away Odds
        }
    }
}
