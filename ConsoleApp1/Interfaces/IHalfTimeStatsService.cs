using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IHalfTimeStatsService
    {
        int GetTotalHalfTimeWins(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalHalfTimeLosses(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalHalfTimeDraw(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalHalfTimeCorners(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalHalfTimeBookings(string teamName, DateTime startSeason, DateTime endSeason);
        double GetTotalHalfTimeGoals(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageHalfTimeGoalsScored(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageHalfTimeCorners(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageHalfTimeBookings(string teamName, DateTime startSeason, DateTime endSeason);

        int GetHeadToHeadHalfTimeWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason);


    }
}
