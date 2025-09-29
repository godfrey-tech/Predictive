using ConsoleApp1.Modals;
using ConsoleApp1.Modals.HistoricalData.PremierLeague;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    public class DataFetcher
    {
        private static readonly HttpClient client = new HttpClient();
        private  string apiKey = "b2bdd266bb9a43ada8d79555fabdd0d2"; // Replace with your API key

        public async Task<MatchStatistics> GetLastNMatchesStats(string teamName, int numberOfMatches, int teamId, string apiKey)
        {
            try
            {
                // Fetch historical matches for the team
                string endpoint = $"https://api.football-data.org/v4/teams/{teamId}/matches"; // Adjust based on your API structure
                var dataFetcher = new DataFetcher();
                var historicalMatches = await dataFetcher.GetHistoricalDataAsync(apiKey, endpoint);

                // Filter and order matches by date
                var filteredMatches = historicalMatches.Matches
                    .Where(match => match.Status.Equals("FINISHED", StringComparison.OrdinalIgnoreCase) &&
                                    (match.HomeTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase) ||
                                     match.AwayTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(match => match.UtcDate) // Order by date descending
                    .Take(numberOfMatches) // Take the last N matches
                    .ToList();

                // Calculate wins, losses, draws, and halftime stats
                var stats = new MatchStatistics();
                foreach (var match in filteredMatches)
                {
                    if (match.Score?.FullTime != null) // Ensure FullTime scores are available
                    {
                        // Calculate full-time stats
                        if (match.HomeTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Team is home
                            if (match.Score.FullTime.Home > match.Score.FullTime.Away)
                                stats.Wins++;
                            else if (match.Score.FullTime.Home < match.Score.FullTime.Away)
                                stats.Losses++;
                            else
                                stats.Draws++;
                        }
                        else
                        {
                            // Team is away
                            if (match.Score.FullTime.Away > match.Score.FullTime.Home)
                                stats.Wins++;
                            else if (match.Score.FullTime.Away < match.Score.FullTime.Home)
                                stats.Losses++;
                            else
                                stats.Draws++;
                        }

                        // Calculate total goals scored
                        int homeGoals = match.Score.FullTime.Home ?? 0; 
                        int awayGoals = match.Score.FullTime.Away ?? 0;
                        stats.TotalGoalsScored += homeGoals + awayGoals;
                        stats.TotalHomeGoalsScored += homeGoals;
                        stats.TotalAwayGoalsScored += awayGoals;
                    }

                    // Calculate halftime stats
                    if (match.Score?.HalfTime != null) // Ensure HalfTime scores are available
                    {
                        if (match.HomeTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Team is home
                            if (match.Score.HalfTime.Home > match.Score.HalfTime.Away)
                                stats.HalfTimeWins++;
                            else if (match.Score.HalfTime.Home < match.Score.HalfTime.Away)
                                stats.HalfTimeLosses++;
                            else
                                stats.HalfTimeDraws++;
                        }
                        else
                        {
                            // Team is away
                            if (match.Score.HalfTime.Away > match.Score.HalfTime.Home)
                                stats.HalfTimeWins++;
                            else if (match.Score.HalfTime.Away < match.Score.HalfTime.Home)
                                stats.HalfTimeLosses++;
                            else
                                stats.HalfTimeDraws++;
                        }
                    }
                }

                return stats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<MatchStatistics> GetLastNMatchesStatsOldV(string teamName, int numberOfMatches, int teamId, string apiKey)
        {
            try
            {
                // Fetch historical matches for the team
                string endpoint = $"https://api.football-data.org/v4/teams/{teamId}/matches"; // Adjust based on your API structure
                var dataFetcher = new DataFetcher();
                var historicalMatches = await dataFetcher.GetHistoricalDataAsync(apiKey, endpoint);

                // Filter matches that are finished and belong to the specified team
                var filteredMatches = historicalMatches.Matches
                    .Where(match => match.Status.Equals("FINISHED", StringComparison.OrdinalIgnoreCase) &&
                                    (match.HomeTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase) ||
                                     match.AwayTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(match => match.UtcDate) // Order by date descending
                    .Take(numberOfMatches) // Take the last N matches
                    .ToList();

                // Calculate wins, losses, and draws
                var stats = new MatchStatistics();
                foreach (var match in filteredMatches)
                {
                    if (match.Score.FullTime.Home.HasValue && match.Score.FullTime.Away.HasValue)
                    {
                        if (match.HomeTeam.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Team is home
                            if (match.Score.FullTime.Home > match.Score.FullTime.Away)
                                stats.Wins++;
                            else if (match.Score.FullTime.Home < match.Score.FullTime.Away)
                                stats.Losses++;
                            else
                                stats.Draws++;
                        }
                        else
                        {
                            // Team is away
                            if (match.Score.FullTime.Away > match.Score.FullTime.Home)
                                stats.Wins++;
                            else if (match.Score.FullTime.Away < match.Score.FullTime.Home)
                                stats.Losses++;
                            else
                                stats.Draws++;
                        }
                    }
                }

                return stats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<List<HistoricalMatchData>> GetHistoricalDataForFixture(string homeTeam, string awayTeam, int homeTeamId)
        {
            // Construct the API endpoint for fetching historical data for the specific teams
            string endpoint = $"https://api.football-data.org/v4/teams/{homeTeamId}/matches"; // Adjust based on your API structure

            // Fetch historical data
            var dataFetcher = new DataFetcher();
            var response = await dataFetcher.GetHistoricalDataAsync(apiKey, endpoint);
            var historicalData = new List<HistoricalMatchData>();

            foreach (var match in response.Matches)
            {
                // Only include matches involving the specified teams
                bool isHeadToHead = (match.HomeTeam.Name.Equals(homeTeam, StringComparison.OrdinalIgnoreCase) && match.AwayTeam.Name.Equals(awayTeam, StringComparison.OrdinalIgnoreCase)) ||
                                    (match.HomeTeam.Name.Equals(awayTeam, StringComparison.OrdinalIgnoreCase) && match.AwayTeam.Name.Equals(homeTeam, StringComparison.OrdinalIgnoreCase));

                if (isHeadToHead)
                {
                    Console.WriteLine($"Match Found: {match.HomeTeam.Name} vs {match.AwayTeam.Name} on {match.UtcDate}");

                    historicalData.Add(new HistoricalMatchData
                    {
                        HomeTeam = match.HomeTeam.Name,
                        AwayTeam = match.AwayTeam.Name,
                        HomeScore = match.Score.FullTime.Home ?? 0, // Handle null
                        AwayScore = match.Score.FullTime.Away ?? 0, // Handle null
                        HomeGoalsConceded = match.Score.FullTime.Away ?? 0,
                        AwayGoalsConceded = match.Score.FullTime.Home ?? 0,
                        Outcome = (match.Score.FullTime.Home > match.Score.FullTime.Away) ? "HomeWin" :
                                  (match.Score.FullTime.Home < match.Score.FullTime.Away) ? "AwayWin" : "Draw",
                        MatchDate = match.UtcDate,
                        Competition = match.Competition.Name,
                        HomeHalfTimeScore = match.Score?.HalfTime?.Home,
                        AwayHalfTimeScore = match.Score?.HalfTime?.Away,
                    });
                }
            }

            return historicalData;
        }

        public async Task<ApiResponse> GetHistoricalDataAsync(string apiKey, string endpoint)
        {
            // Define a Polly retry policy
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => r.StatusCode == (System.Net.HttpStatusCode)429)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMinutes(1),
                    (result, timeSpan, context) =>
                    {
                        if (result.Exception != null)
                        {
                            Console.WriteLine($"Request failed with exception: {result.Exception.Message}. Waiting {timeSpan} before next retry.");
                        }
                        else
                        {
                            Console.WriteLine($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry.");
                        }
                    });

            try
            {
                client.DefaultRequestHeaders.Clear(); // Clear existing headers
                client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);

                // Execute the request with the retry policy
                var response = await retryPolicy.ExecuteAsync(async () =>
                {
                    var httpResponse = await client.GetAsync(endpoint);
                    httpResponse.EnsureSuccessStatusCode(); // Throw if not a success code.
                    return httpResponse;
                });

                // If the request was successful, read the content and deserialize it
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception after logging
            }
        }
        public async Task<ApiResponse> GetHistoricalDataAsyncOldV(string apiKey, string endpoint)
        {
            try
            {
                client.DefaultRequestHeaders.Clear(); // Clear existing headers
                client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);
                var response = await client.GetStringAsync(endpoint);

                // Deserialize the JSON response into ApiResponse object
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                return apiResponse;
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception after logging
            }
        }
        public async Task<ApiResponse> GetUpcomingFixturesDataAsync(string apiKey, string endpoint)
        {
            try
            {
                client.DefaultRequestHeaders.Clear(); // Clear existing headers
                client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);
                var response = await client.GetStringAsync(endpoint);

                // Deserialize the JSON response into ApiResponse object
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                return apiResponse;
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine($"Request error: {httpRequestException.Message}");
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception after logging
            }
        }
    }
}
