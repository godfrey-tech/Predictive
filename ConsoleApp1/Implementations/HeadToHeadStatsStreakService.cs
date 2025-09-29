using ConsoleApp1.Interfaces;
using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Implementations
{
    public class HeadToHeadStatsStreakService : IHeadToHeadStatsStreakService
    {
        private readonly List<SeasonMatchRecord> _matchData; //this must be season league data
        public HeadToHeadStatsStreakService(List<SeasonMatchRecord> matchData)
        {
            _matchData = matchData;
        }
        public int GetCurrentWinStreak(string teamA, string teamB)
        {
            var headToHeadMatches = _matchData
                .Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) || (x.HomeTeam == teamB && x.AwayTeam == teamA))
                .OrderByDescending(x => x.Date) // Assuming MatchDate is a DateTime property in SeasonMatchRecord
                .ToList();

            int currentStreak = 0;

            foreach (var match in headToHeadMatches)
            {
                if ((match.HomeTeam == teamA && match.HomeGoals > match.AwayGoals) ||
                    (match.AwayTeam == teamA && match.AwayGoals > match.HomeGoals))
                {
                    currentStreak++;
                }
                else
                {
                    break; // Streak ends on the first non-win
                }
            }

            return currentStreak;
        }

        public int GetCurrentLossStreak(string teamA, string teamB)
        {
            var headToHeadMatches = _matchData
                .Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) || (x.HomeTeam == teamB && x.AwayTeam == teamA))
                .OrderByDescending(x => x.Date)
                .ToList();

            int currentStreak = 0;

            foreach (var match in headToHeadMatches)
            {
                if ((match.HomeTeam == teamA && match.HomeGoals < match.AwayGoals) ||
                    (match.AwayTeam == teamA && match.AwayGoals < match.HomeGoals))
                {
                    currentStreak++;
                }
                else
                {
                    break; // Streak ends on the first non-loss
                }
            }

            return currentStreak;
        }

        public int GetLongestWinStreak(string teamA, string teamB)
        {
            var headToHeadMatches = _matchData
                .Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) || (x.HomeTeam == teamB && x.AwayTeam == teamA))
                .OrderBy(x => x.Date)
                .ToList();

            int longestStreak = 0;
            int currentStreak = 0;

            foreach (var match in headToHeadMatches)
            {
                if ((match.HomeTeam == teamA && match.HomeGoals > match.AwayGoals) ||
                    (match.AwayTeam == teamA && match.AwayGoals > match.HomeGoals))
                {
                    currentStreak++;
                }
                else
                {
                    longestStreak = Math.Max(longestStreak, currentStreak);
                    currentStreak = 0; // Reset current streak
                }
            }

            // Check at the end in case the longest streak is at the end of the list
            longestStreak = Math.Max(longestStreak, currentStreak);

            return longestStreak;
        }

        public int GetLongestLossStreak(string teamA, string teamB)
        {
            var headToHeadMatches = _matchData
                .Where(x => (x.HomeTeam == teamA && x.AwayTeam == teamB) || (x.HomeTeam == teamB && x.AwayTeam == teamA))
                .OrderBy(x => x.Date)
                .ToList();

            int longestStreak = 0;
            int currentStreak = 0;

            foreach (var match in headToHeadMatches)
            {
                if ((match.HomeTeam == teamA && match.HomeGoals < match.AwayGoals) ||
                    (match.AwayTeam == teamA && match.AwayGoals < match.HomeGoals))
                {
                    currentStreak++;
                }
                else
                {
                    longestStreak = Math.Max(longestStreak, currentStreak);
                    currentStreak = 0; // Reset current streak
                }
            }

            // Check at the end in case the longest streak is at the end of the list
            longestStreak = Math.Max(longestStreak, currentStreak);

            return longestStreak;
        }

    }
}
