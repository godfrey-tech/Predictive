using Microsoft.ML;
using Newtonsoft.Json;
using Predictvie.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Services
{
    public class PredictionModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public PredictionModel()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<MatchData> trainingData)
        {
            var trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Define the training pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features",
                    "HomeTeamForm",
                    "AwayTeamForm",
                    "HomeTeamAverageCorners",
                    "AwayTeamAverageCorners",
                    "HomeTeamAverageBookings",
                    "AwayTeamAverageBookings",
                    "IsHomeGame")
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Corners", maximumNumberOfIterations: 100));

            // Train the model
            _model = pipeline.Fit(trainingDataView);
        }

        public PredictionResult Predict(MatchData matchData)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MatchData, PredictionResult>(_model);
            var prediction = predictionEngine.Predict(matchData);
            return prediction;
        }
    }

}
