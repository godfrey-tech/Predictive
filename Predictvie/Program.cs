// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Predictvie.APIs;
using Predictvie.Modals;
using Predictvie.Services;

Console.WriteLine("Hello, World!");
var apiClient = new SportsApiClient();
string apiUrl = "your_api_url_here"; // Replace with your actual API URL
string jsonData = await apiClient.GetDataAsync(apiUrl);

List<MatchData> trainingData = ParseMatchData(jsonData);
var predictionModel = new PredictionModel();
predictionModel.TrainModel(trainingData);

// Example match data for prediction
var newMatchData = new MatchData
{
    HomeTeamForm = 2.5f,
    AwayTeamForm = 1.8f,
    HomeTeamAverageCorners = 5.0f,
    AwayTeamAverageCorners = 4.0f,
    HomeTeamAverageBookings = 2.0f,
    AwayTeamAverageBookings = 1.5f,
    IsHomeGame = true // true for home game, false for away game
};

var prediction = predictionModel.Predict(newMatchData);
Console.WriteLine($"Predicted Corners: {prediction.PredictedCorners}, Predicted Bookings: {prediction.PredictedBookings}");


static List<MatchData> ParseMatchData(string jsonData)
{
    return JsonConvert.DeserializeObject<List<MatchData>>(jsonData);
}
Console.ReadLine();