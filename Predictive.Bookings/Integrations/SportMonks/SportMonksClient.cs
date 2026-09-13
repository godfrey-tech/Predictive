using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Predictive.Bookings.Integrations.SportMonks
{
    // Thin HTTP wrapper around the SportMonks v3 football API. Centralizes the one
    // HttpClient and token resolution here rather than the `new HttpClient()`-per-call
    // style used elsewhere in this codebase (LeagueCsvDownloadService,
    // DataFetcherService), since this will be called far more often.
    public class SportMonksClient
    {
        private static readonly HttpClient Http = new HttpClient
        {
            BaseAddress = new Uri("https://api.sportmonks.com/v3/")
        };

        private readonly string _apiToken;

        public SportMonksClient(string? apiToken = null)
        {
            _apiToken = apiToken ?? ResolveToken();
        }

        // Never hardcode the token in source (unlike Program.cs:27's football-data.org
        // key elsewhere in this codebase) — read it from an environment variable first,
        // then a git-ignored appsettings.local.json (see appsettings.local.json.example
        // for the shape). Throws with clear setup instructions if neither is found.
        private static string ResolveToken()
        {
            var envToken = Environment.GetEnvironmentVariable("SPORTMONKS_API_TOKEN");
            if (!string.IsNullOrEmpty(envToken)) return envToken;

            var configPath = ResolveLocalConfigPath();
            if (configPath != null)
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (doc.RootElement.TryGetProperty("SportMonks", out var sportMonks) &&
                    sportMonks.TryGetProperty("ApiToken", out var tokenElement))
                {
                    var token = tokenElement.GetString();
                    if (!string.IsNullOrEmpty(token) && token != "YOUR_TOKEN_HERE")
                        return token;
                }
            }

            throw new InvalidOperationException(
                "No SportMonks API token found. Either set the SPORTMONKS_API_TOKEN environment " +
                "variable, or copy Predictive.Bookings/appsettings.local.json.example to " +
                "appsettings.local.json and fill in ApiToken.");
        }

        private static string? ResolveLocalConfigPath()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "appsettings.local.json");
                if (File.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            return null;
        }

        public async Task<JsonDocument> GetAsync(string path)
        {
            var url = path.Contains('?')
                ? $"{path}&api_token={_apiToken}"
                : $"{path}?api_token={_apiToken}";

            var response = await Http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonDocument.ParseAsync(stream);
        }
    }
}
