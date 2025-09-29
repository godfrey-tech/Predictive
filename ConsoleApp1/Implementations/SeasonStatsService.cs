using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class SeasonStatsService : ISeasonStatsService
    {
        private readonly List<SeasonMatchRecord> _matchData; //this must be season league data
        public SeasonStatsService(List<SeasonMatchRecord> matchData)
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

        public int GetHeadToHeadDraw(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => x.HomeGoals == x.AwayGoals);
        }

        public List<SeasonMatchRecord> GetRecentMatches(string teamName, DateTime startSeason, DateTime endSeason, int count)
        {
            return _matchData
                .Where(x => (x.HomeTeam == teamName || x.AwayTeam == teamName)
                             && x.Date < endSeason
                             && x.Date >= startSeason)
                .OrderByDescending(x => x.Date)
                .Take(count)
                .ToList();
        }

        public int GetHeadToHeadLosses(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return GetHeadToHeadMatches(teamA, teamB, startSeason, endSeason).Count(x => (x.HomeTeam == teamA && x.HomeGoals < x.AwayGoals) || 
                    x.AwayTeam == teamA && x.AwayGoals < x.HomeGoals);
        }
        public int GetTotalBookings(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                              .Sum(x => x.HomeTeam == teamName ? x.HomeBookings 
                                    : x.AwayTeam == teamName ? x.AwayBookings : 0);
        }

        public int GetTotalGoals(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(m => m.Date >= startSeason && m.Date <= endSeason)
                             .Sum(m => m.HomeTeam == teamName ? m.HomeGoals : m.AwayTeam == teamName ? m.AwayGoals : 0);
        }

        public int GetTotalCorners(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                              .Sum(x => x.HomeTeam == teamName ? x.HomeCorners
                                     : x.AwayTeam == teamName ? x.AwayCorners : 0);
        }
        public int GetTotalDraw(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => (x.HomeTeam == teamName || x.AwayTeam == teamName) && x.HomeGoals == x.AwayGoals)
                             .Where(x => x.Date >= startSeason && x.Date <= endSeason)
                             .Count();
        }

        public int GetTotalLosses(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => (x.HomeTeam == teamName || x.AwayTeam == teamName))
                             .Where(m => m.Date >= startSeason && m.Date <= endSeason)
                             .Where(x => (x.HomeTeam == teamName && x.HomeGoals < x.AwayGoals) 
                                    || (x.AwayTeam == teamName && x.AwayGoals < x.HomeGoals)).Count();
        }

        public int GetTotalWins(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => (x.HomeTeam == teamName || x.AwayTeam == teamName))
                             .Where(m => m.Date >= startSeason && m.Date <= endSeason)
                             .Where(x => (x.HomeTeam == teamName && x.HomeGoals > x.AwayGoals) 
                                    || (x.AwayTeam == teamName && x.AwayGoals > x.HomeGoals)).Count();
        }

        public double GetAverageGoalsConceded(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var teamMatches = _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                                        .Where(x => x.Date >= startSeason && x.Date <= endSeason).ToList();
            if (!teamMatches.Any()) return 0;

            // Calculate total goals conceded
            var totalGoalsConceded = teamMatches.Sum(x => x.HomeTeam == teamName ? x.AwayGoals : x.HomeGoals);

            return (double)totalGoalsConceded / (double)teamMatches.Count();
        }
        public List<SeasonMatchRecord> GetHomeMatches(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData
                .Where(x => x.HomeTeam == teamName && x.Date >= startSeason && x.Date <= endSeason)
                .ToList();
        }
        public List<SeasonMatchRecord> GetAwayMatches(string teamName, DateTime startSeason, DateTime endSeason)
        {
            return _matchData
                .Where(x => x.AwayTeam == teamName && x.Date >= startSeason && x.Date <= endSeason)
                .ToList();
        }

        public double GetAverageGoalsScored(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var teamMatches = _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                                      .Where(x => x.Date >= startSeason && x.Date <= endSeason).ToList();
            if (!teamMatches.Any()) return 0;
            var totalGoals = teamMatches.Sum(x => x.HomeTeam == teamName ? x.HomeGoals : x.AwayGoals);
            return (double)totalGoals / (double)teamMatches.Count();
        }

        public double GetAverageCorners(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var teamMatches = _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                                      .Where(x => x.Date >= startSeason && x.Date <= endSeason).ToList();
            if (!teamMatches.Any()) return 0;
            var totalCorners = teamMatches.Sum(x => x.HomeTeam == teamName ? x.HomeCorners : x.AwayCorners);
            return (double)totalCorners / (double)teamMatches.Count();
        }

        public double GetAverageBookings(string teamName, DateTime startSeason, DateTime endSeason)
        {
            var teamMatches = _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                                        .Where(x => x.Date >= startSeason && x.Date <= endSeason).ToList();
            if (!teamMatches.Any()) return 0;
            var totalBookings = teamMatches.Sum(x => x.HomeTeam == teamName ? x.HomeBookings : x.AwayBookings);
            return (double)totalBookings / (double)teamMatches.Count();
        }
    }
}
