using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IHeadToHeadStatsService
    {
        List<SeasonMatchRecord> GetHeadToHeadMatches(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        List<SeasonMatchRecord> GetHeadToHeadNMatchesBetweenTeams(int n);
        int GetHeadToHeadWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        double GetHeadToHeadAverageGoals(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadCorner(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeCorner(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadBookings(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
        int GetHeadToHeadHalfTimeBookings(string teamA, string teamB, DateTime startSeason, DateTime endSeason);
    }
}
