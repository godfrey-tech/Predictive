using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class HalfTimeStatsService : IHalfTimeStatsService
    {
        private readonly List<SeasonMatchRecord> _matchData;

        public HalfTimeStatsService(List<SeasonMatchRecord> matchData)
        {
            _matchData = matchData;
        }

        public int GetTotalHalfTimeWins(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                (x.HomeTeam == teamName && x.HomeHalfTimeGoals > x.AwayHalfTimeGoals) ||
                (x.AwayTeam == teamName && x.AwayHalfTimeGoals > x.HomeHalfTimeGoals) &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }

        public int GetTotalHalfTimeLosses(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                (x.HomeTeam == teamName && x.HomeHalfTimeGoals < x.AwayHalfTimeGoals) ||
                (x.AwayTeam == teamName && x.AwayHalfTimeGoals < x.HomeHalfTimeGoals) &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }

        public int GetTotalHalfTimeDraw(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                (x.HomeTeam == teamName || x.AwayTeam == teamName) &&
                x.HomeHalfTimeGoals == x.AwayHalfTimeGoals &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }

        public int GetTotalHalfTimeCorners(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                             .Sum(x => x.HomeTeam == teamName ? x.HomeHalfTimeCorners : x.AwayHalfTimeCorners);
        }

        public int GetTotalHalfTimeBookings(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                             .Sum(x => x.HomeTeam == teamName ? x.HomeHalfTimeBookings : x.AwayHalfTimeBookings);
        }

        public double GetTotalHalfTimeGoals(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                             .Sum(x => x.HomeTeam == teamName ? x.HomeHalfTimeGoals : x.AwayHalfTimeGoals);
        }

        public double GetAverageHalfTimeGoalsScored(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var totalHalfTimeGoals = GetTotalHalfTimeGoals(teamName, startSeason, endSeason);
            var totalMatches = _matchData.Count(x =>
                (x.HomeTeam == teamName || x.AwayTeam == teamName) &&
                (x.Date >= startSeason && x.Date <= endSeason));
            return totalMatches == 0 ? 0 : (double)totalHalfTimeGoals / totalMatches;
        }

        public double GetAverageHalfTimeCorners(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var totalHalfTimeCorners = GetTotalHalfTimeCorners(teamName, startSeason, endSeason);
            var totalMatches = _matchData.Count(x =>
                (x.HomeTeam == teamName || x.AwayTeam == teamName) &&
                (x.Date >= startSeason && x.Date <= endSeason));
            return totalMatches == 0 ? 0 : (double)totalHalfTimeCorners / totalMatches;
        }

        public double GetAverageHalfTimeBookings(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var totalHalfTimeBookings = GetTotalHalfTimeBookings(teamName, startSeason, endSeason);
            var totalMatches = _matchData.Count(x =>
                (x.HomeTeam == teamName || x.AwayTeam == teamName) &&
                (x.Date >= startSeason && x.Date <= endSeason));
            return totalMatches == 0 ? 0 : (double)totalHalfTimeBookings / totalMatches;
        }

        public int GetHeadToHeadHalfTimeWins(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                (x.HomeTeam == teamA && x.AwayTeam == teamB && x.HomeHalfTimeGoals > x.AwayHalfTimeGoals) ||
                (x.HomeTeam == teamB && x.AwayTeam == teamA && x.AwayHalfTimeGoals > x.HomeHalfTimeGoals) &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }

        public int GetHeadToHeadHalfTimeDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                ((x.HomeTeam == teamA && x.AwayTeam == teamB) ||
                 (x.HomeTeam == teamB && x.AwayTeam == teamA)) &&
                (x.HomeHalfTimeGoals == x.AwayHalfTimeGoals) &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }

        public int GetHeadToHeadHalfTimeLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Count(x =>
                (x.HomeTeam == teamA && x.AwayTeam == teamB && x.HomeHalfTimeGoals < x.AwayHalfTimeGoals) ||
                (x.HomeTeam == teamB && x.AwayTeam == teamA && x.AwayHalfTimeGoals < x.HomeHalfTimeGoals) &&
                (x.Date >= startSeason && x.Date <= endSeason));
        }
    }
}
