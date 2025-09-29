using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals.HistoricalData.PremierLeague
{
    public class HistoricalMatchFromCSVFileExtraLeague
    {
        public string Country { get; set; }
        public string League { get; set; }
        public string Season { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public int? HG { get; set; } // Home Goals
        public int? AG { get; set; } // Away Goals
        public string Res { get; set; } // Result
        public double? PSCH { get; set; } // Pinnacle Sports Correct Score Home Odds
        public double? PSCD { get; set; } // Pinnacle Sports Correct Score Draw Odds
        public double? PSCA { get; set; } // Pinnacle Sports Correct Score Away Odds
        public double? MaxCH { get; set; } // Max Correct Score Home Odds
        public double? MaxCD { get; set; } // Max Correct Score Draw Odds
        public double? MaxCA { get; set; } // Max Correct Score Away Odds
        public double? AvgCH { get; set; } // Avg Correct Score Home Odds
        public double? AvgCD { get; set; } // Avg Correct Score Draw Odds
        public double? AvgCA { get; set; } // Avg Correct Score Away Odds
        public double? BFECH { get; set; } // Betfair Correct Score Home Odds
        public double? BFECD { get; set; } // Betfair Correct Score Draw Odds
        public double? BFECA { get; set; } // Betfair Correct Score Away Odds
    }
}
