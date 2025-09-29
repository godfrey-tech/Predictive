using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface ITrendAnalyzerService
    {
        int GetWinningStreak(string team);
        int GetScoringStreak(string team);
        int GetCleanSheetStreak(string team);
        int GetOver25GoalTrend(string team);
    }
}
