using ConsoleApp1.Modals;
using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IMatchOutcomePredictor
    {
        PredictOutcome PredictOutcome(string homeTeam, string awayTeam, DateTime startSeason, DateTime endSeason);
        TeamStatistics GetTeamStatistics(string team, DateTime startSeason, DateTime endSeason);
    
    }
}
