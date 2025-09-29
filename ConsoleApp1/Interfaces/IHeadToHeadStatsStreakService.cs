using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IHeadToHeadStatsStreakService
    {
        int GetCurrentWinStreak(string teamA, string teamB);
        int GetCurrentLossStreak(string teamA, string teamB);
        int GetLongestWinStreak(string teamA, string teamB);
        int GetLongestLossStreak(string teamA, string teamB);
    }

}
