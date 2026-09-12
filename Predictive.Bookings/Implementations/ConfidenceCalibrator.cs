using System;
using System.Collections.Generic;
using System.Linq;

namespace Predictive.Bookings.Implementations
{
    // 1-D Platt scaling: fits calibrated = sigmoid(a * (rawPct/100) + b) via batch
    // gradient descent on log-loss. Corrects the case where the engine's raw
    // hit-rate says "75%" but those bets actually win at a different real rate.
    public class ConfidenceCalibrator
    {
        private double _a = 1.0;
        private double _b = 0.0;
        private bool _fitted = false;

        public bool IsFitted => _fitted;

        public void Fit(List<(double rawPct, bool won)> samples, int iterations = 2000, double learningRate = 0.1)
        {
            if (samples.Count < 30) return; // too few points to trust a fit

            double a = 1.0, b = 0.0;
            int n = samples.Count;

            for (int iter = 0; iter < iterations; iter++)
            {
                double gradA = 0, gradB = 0;
                foreach (var (rawPct, won) in samples)
                {
                    double x = rawPct / 100.0;
                    double p = Sigmoid(a * x + b);
                    double error = p - (won ? 1.0 : 0.0); // d(log-loss)/dz
                    gradA += error * x;
                    gradB += error;
                }
                a -= learningRate * gradA / n;
                b -= learningRate * gradB / n;
            }

            _a = a;
            _b = b;
            _fitted = true;
        }

        public double Calibrate(double rawPct)
        {
            if (!_fitted) return rawPct;
            double x = rawPct / 100.0;
            return Sigmoid(_a * x + _b) * 100.0;
        }

        private static double Sigmoid(double z) => 1.0 / (1.0 + Math.Exp(-z));
    }
}
