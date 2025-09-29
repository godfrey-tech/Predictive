using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface ISeasonStatsService
    {
        int GetTotalWins(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalLosses(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalDraw(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalCorners(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalGoals(string teamName, DateTime startSeason, DateTime endSeason);
        int GetTotalBookings(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageGoalsScored(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageGoalsConceded(string teamName, DateTime startSeason, DateTime endSeason);
        double GetAverageCorners(string teamName, DateTime startSeason, DateTime endSeason);
        List<SeasonMatchRecord> GetHomeMatches(string teamName, DateTime startSeason, DateTime endSeason);
        List<SeasonMatchRecord> GetAwayMatches(string teamName, DateTime startSeason, DateTime endSeason);
        List<SeasonMatchRecord> GetRecentMatches(string teamName, DateTime startSeason, DateTime endSeason, int count);

        double GetAverageBookings(string teamName, DateTime startSeason, DateTime endSeason);
        
        List<SeasonMatchRecord> GetHeadToHeadMatches(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason);

        int GetHeadToHeadLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason);


    }
}
