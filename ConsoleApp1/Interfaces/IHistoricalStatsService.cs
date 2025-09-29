using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IHistoricalStatsService
    {
        double GetAverageGoalsPerSeason(string teamName, int startSeason, int endSeason);
        List<string> GetTopRivalries(string teamName);
    }
}
