using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IMatchContextService
    {
        TeamForm GetRecentForm(string teamName, int numberOfMatches);
        HeadToHead GetHeadoHead(string homeTeam, string awayTeam, DateTime startSeason, DateTime endSeason);
        MatchSummary GetMatchSummary(string homeTeam, string awayTeam, DateTime matchDate);


    }
}
