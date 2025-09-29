using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class MatchContextService : IMatchContextService
    {
        private readonly List<SeasonMatchRecord> _matchData;
        public MatchContextService(List<SeasonMatchRecord> matchData)
        {
            _matchData = matchData;
        }

        public List<SeasonMatchRecord> GetHeadToHeadMatches(string teamA, string teamB, DateTime startSeason, DateTime endSeason)
        {
            return _matchData.Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) ||
                                         (x.HomeTeam == teamB && x.AwayTeam == teamA))
                                         .Where(x => (x.Date >= startSeason) && (x.Date <= endSeason)).ToList();
        }
        public HeadToHead GetHeadoHead(string homeTeam, string awayTeam, DateTime startSeason, DateTime endSeason)
        {
            var headToHead = GetHeadToHeadMatches(homeTeam, awayTeam, startSeason, endSeason).ToList();
            int homeWins = 0, awayWins = 0, draws = 0;
            foreach (var match in _matchData)
            {
                if (match.HomeGoals > match.AwayGoals)
                {
                    if (match.HomeTeam == homeTeam) homeWins++;
                    else awayWins++;
                }
                else if (match.HomeGoals < match.AwayGoals)
                {
                    if (match.AwayTeam == homeTeam) homeWins++;
                    else awayWins++;
                }
                else
                { 
                    draws++;
                }
            }
            return new HeadToHead()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                TotalMatches = headToHead.Count,
                HomeWins = homeWins,
                AwayWins = awayWins,
                Draws = draws,
            };
        }

        public MatchSummary GetMatchSummary(string homeTeam, string awayTeam, DateTime startSeason, DateTime endSeason, DateTime matchDate)
        {
            var match = _matchData.FirstOrDefault(x => (x.HomeTeam == homeTeam && x.AwayTeam == awayTeam) && (x.Date >= startSeason && x.Date <= endSeason) && x.Date == matchDate);
            if (match == null) return null;
            return new MatchSummary()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                MatchDate = matchDate,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                TotalCorners = 1,
                TotalBookings = 1
            };
        }

        public TeamForm GetRecentForm(string teamName, int numberOfMatches)
        {
            var recentMatches = _matchData.Where(x => x.HomeTeam == teamName || x.AwayTeam == teamName)
                                .OrderByDescending(x => x.Date).Take(numberOfMatches);
            int win = 0, draw = 0, losses = 0, scored = 0, conceded = 0;
            foreach (var match in recentMatches)
            {
                bool isHome = match.HomeTeam == teamName;
                int teamGoals = isHome ? match.HomeGoals : match.AwayGoals;
                int opponenentGoals = isHome ? match.AwayGoals : match.HomeGoals;
                scored += teamGoals;
                conceded += opponenentGoals;
                if (teamGoals > opponenentGoals)
                {
                    win++;
                }
                else if(teamGoals == opponenentGoals) 
                { 
                    draw++;
                }
                else
                {
                   losses++;
                }
            }
            return new TeamForm()
            {
                TeamName = teamName,
                Win = win,
                Draws = draw,
                Losses = losses,
                GoalScored = scored,
                GoalConceded = conceded
            };
        }

        public MatchSummary GetMatchSummary(string homeTeam, string awayTeam, DateTime matchDate)
        {
            throw new NotImplementedException();
        }
    }
}
