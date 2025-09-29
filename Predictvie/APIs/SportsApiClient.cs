using Newtonsoft.Json;
using Predictvie.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Predictvie.APIs
{
    public class SportsApiClient
    {
       
        private readonly HttpClient _httpClient;

        public SportsApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetDataAsync(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
