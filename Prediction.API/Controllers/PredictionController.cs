using Microsoft.AspNetCore.Mvc;

namespace Prediction.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly ApiService _apiService;
        private readonly PredictionModel _predictionModel;

        public PredictionController()
        {
            _apiService = new ApiService();
            _predictionModel = new PredictionModel();
        }

        [HttpGet("train")]
        public async Task<IActionResult> TrainModel(string apiUrl)
        {
            string jsonData = await _apiService.GetDataAsync(apiUrl);
            List<MatchData> trainingData = ParseMatchData(jsonData);
            _predictionModel.TrainModel(trainingData);
            return Ok("Model trained successfully.");
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] MatchData newMatchData)
        {
            var prediction = _predictionModel.Predict(newMatchData);
            return Ok(prediction);
        }

        private List<MatchData> ParseMatchData(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<MatchData>>(jsonData);
        }


    }
}
