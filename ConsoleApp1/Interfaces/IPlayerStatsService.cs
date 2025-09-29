using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IPlayerStatsService
    {
        int GetTotalGoals(string playerName, int startSeason, int endSeason);
        int GetTotalAssists(string playerName, int startSeason, int endSeason);
        int GetTotalBookings(string playerName, int startSeason, int endSeason);
        double GetAverageGoalsPerMatch(string playerName, int startSeason, int endSeason);
    }
}
