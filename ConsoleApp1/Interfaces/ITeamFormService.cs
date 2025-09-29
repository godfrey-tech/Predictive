using ConsoleApp1.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface ITeamFormService
    {
        List<SeasonMatchRecord> GetRecentMatches(string teamName, int numberOfMatches);
        double GetWinPercentage(string teamName, int startSeason, int endSeason);
    }
}
