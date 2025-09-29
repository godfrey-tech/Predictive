using ConsoleApp1.Modals;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1.Services
{
    public class DataProcessor
    {
        //public List<MatchData> ProcessData(ApiResponse apiResponse)
        //{
        //    var matchDataList = new List<MatchData>();

        //    foreach (var match in apiResponse.Matches)
        //    {
        //        var homeTeam = match.HomeTeam.Name;
        //        var awayTeam = match.AwayTeam.Name;
        //        var homeScore = match.Score.FullTime.Home; // Nullable int
        //        var awayScore = match.Score.FullTime.Away; // Nullable int

        //        // Determine outcome based on scores
        //        var outcome = (homeScore, awayScore) switch
        //        {
        //            (null, null) => null, // If scores are not available
        //            (_, null) => "HomeWin", // If away score is not available, assume home win
        //            (null, _) => "AwayWin", // If home score is not available, assume away win
        //            _ when homeScore > awayScore => "HomeWin",
        //            _ when homeScore < awayScore => "AwayWin",
        //            _ => "Draw"
        //        };

        //        matchDataList.Add(new MatchData
        //        {
        //            HomeTeam = homeTeam,
        //            AwayTeam = awayTeam,
        //            HomeScore = (float)homeScore,
        //            AwayScore = (float)awayScore,
        //            Outcome = outcome
        //        });
        //    }

        //    return matchDataList;
        //}

    }

}
