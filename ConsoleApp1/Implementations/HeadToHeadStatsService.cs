using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class HeadToHeadStatsService : IHeadToHeadStatsService
    {
        private readonly List<SeasonMatchRecord> _matchData; //this must be season league data
        public HeadToHeadStatsService(List<SeasonMatchRecord> matchData)
        {
            _matchData = matchData;
        }
        public List<SeasonMatchRecord> GetHeadToHeadMatches(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) ||
                                         (x.HomeTeam == teamB && x.AwayTeam == teamA))
                                         .Where(x => (x.Date >= startSeason) && (x.Date <= endSeason)).ToList();
        }

        public int GetHeadToHeadWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(m => (m.HomeTeam == teamA && m.HomeGoals > m.AwayGoals) ||
                                        (m.AwayTeam == teamA && m.AwayGoals > m.HomeGoals));
        }

        public int GetHeadToHeadHalfTimeWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(m => (m.HomeTeam == teamA && m.HomeHalfTimeGoals > m.AwayHalfTimeGoals) ||
                                        (m.AwayTeam == teamA && m.AwayHalfTimeGoals > m.HomeHalfTimeGoals));
        }
        public double GetHeadToHeadAverageGoals(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            // Get all head-to-head matches between the two teams
            var matches = GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason);

            // Check if there are no matches to avoid division by zero
            if (matches.Count == 0) return 0;

            // Calculate total goals scored in these matches
            int totalGoals = matches.Sum(m => m.HomeTeam == teamA ? m.HomeGoals : m.AwayGoals);

            // Calculate and return the average goals
            return (double)totalGoals / matches.Count;
        }
        public int GetHeadToHeadDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => x.HomeGoals == x.AwayGoals);
        }
        public int GetHeadToHeadHalfTimeDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => x.HomeHalfTimeGoals == x.AwayHalfTimeGoals);
        }
        public int GetHeadToHeadLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => (x.HomeTeam == teamA && x.HomeGoals < x.AwayGoals) ||
                    x.AwayTeam == teamA && x.AwayGoals < x.HomeGoals);
        }
        public int GetHeadToHeadHalfTimeLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => (x.HomeTeam == teamA && x.HomeHalfTimeGoals < x.AwayHalfTimeGoals) ||
                    x.AwayTeam == teamA && x.AwayHalfTimeGoals < x.HomeHalfTimeGoals);
        }

        public List<SeasonMatchRecord> GetHeadToHeadNMatchesBetweenTeams(int n)
        {
            throw new NotImplementedException();
        }

        public int GetHeadToHeadCorner(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Sum(m =>
                      (m.HomeTeam == teamA ? m.HomeCorners : m.AwayCorners));
        }

        public int GetHeadToHeadHalfTimeCorner(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Sum(m =>
             (m.HomeTeam == teamA ? m.HomeHalfTimeCorners : m.AwayHalfTimeCorners));
        }

        public int GetHeadToHeadBookings(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Sum(m =>
             (m.HomeTeam == teamA ? m.HomeBookings : m.AwayBookings));
        }

        public int GetHeadToHeadHalfTimeBookings(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Sum(m =>
           (m.HomeTeam == teamA ? m.HomeHalfTimeBookings : m.AwayHalfTimeBookings));
        }
    }
}
