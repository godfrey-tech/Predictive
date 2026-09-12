using System;
using System.Collections.Generic;
using System.Linq;
using Predictive.Bookings.Interfaces;
using Predictive.Bookings.Modals;

namespace Predictive.Bookings.Implementations
{
    // Combines the day's single-match signals into 2-3 leg accumulators.
    // Only draws legs from the "total match cards" markets, since those are the
    // only ones IOddsProvider currently prices (GetCardsOdds is a total-cards
    // quote, not a team-specific one) — team-card markets aren't included here.
    public class CardAccumulatorBuilder
    {
        private readonly BookingsEngine _engine;
        private readonly IOddsProvider? _oddsProvider;
        private const double MIN_LEG_CONFIDENCE = 70.0; // %
        private const int MAX_LEGS = 3;

        public CardAccumulatorBuilder(BookingsEngine engine, IOddsProvider? oddsProvider = null)
        {
            _engine = engine;
            _oddsProvider = oddsProvider;
        }

        public List<AccumulatorCandidate> BuildTopAccumulators(
            List<(string homeTeam, string awayTeam)> fixtures, int maxCandidates = 5)
        {
            var candidateLegs = fixtures
                .Select(f => BestLegFor(f.homeTeam, f.awayTeam))
                .Where(leg => leg != null)
                .Select(leg => leg!)
                .ToList();

            var accumulators = new List<AccumulatorCandidate>();
            int maxSize = Math.Min(MAX_LEGS, candidateLegs.Count);
            for (int size = 2; size <= maxSize; size++)
            {
                foreach (var combo in Combinations(candidateLegs, size))
                {
                    accumulators.Add(BuildCandidate(combo));
                }
            }

            return accumulators
                .OrderByDescending(a => a.ExpectedValue ?? a.CombinedProbability)
                .Take(maxCandidates)
                .ToList();
        }

        // Highest-probability qualifying total-cards market for this fixture, or
        // null when nothing clears MIN_LEG_CONFIDENCE (e.g. thin historical sample).
        private AccumulatorLeg? BestLegFor(string homeTeam, string awayTeam)
        {
            var pred = _engine.Predict(homeTeam, awayTeam);
            var candidates = new List<(string market, double line, double pct)>
            {
                ("Total Cards Over 3", 2.5, pred.FtOver3),
                ("Total Cards Over 4", 3.5, pred.FtOver4),
                ("Total Cards Over 5", 4.5, pred.FtOver5),
                ("Total Cards Over 6", 5.5, pred.FtOver6),
            };

            var best = candidates
                .Where(c => c.pct >= MIN_LEG_CONFIDENCE)
                .OrderByDescending(c => c.pct)
                .FirstOrDefault();

            if (best.market == null) return null;

            var quote = _oddsProvider?.GetCardsOdds(homeTeam, awayTeam, best.line);
            return new AccumulatorLeg
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Market = best.market,
                Line = best.line,
                Probability = best.pct / 100.0,
                Odds = quote?.OverOdds
            };
        }

        private static AccumulatorCandidate BuildCandidate(List<AccumulatorLeg> legs)
        {
            double combinedProbability = legs.Aggregate(1.0, (acc, leg) => acc * leg.Probability);

            double? combinedOdds = legs.All(l => l.Odds.HasValue)
                ? legs.Aggregate(1.0, (acc, leg) => acc * leg.Odds!.Value)
                : null;

            var candidate = new AccumulatorCandidate
            {
                Legs = legs,
                CombinedProbability = combinedProbability,
                CombinedOdds = combinedOdds
            };

            if (combinedOdds.HasValue)
            {
                candidate.ExpectedValue = EvKellyCalculator.CalculateEV(combinedProbability, combinedOdds.Value);
                candidate.KellyFraction = EvKellyCalculator.CalculateKellyFraction(combinedProbability, combinedOdds.Value);
            }

            return candidate;
        }

        private static IEnumerable<List<T>> Combinations<T>(List<T> items, int size)
        {
            if (size == 0) { yield return new List<T>(); yield break; }
            for (int i = 0; i <= items.Count - size; i++)
            {
                foreach (var rest in Combinations(items.Skip(i + 1).ToList(), size - 1))
                {
                    rest.Insert(0, items[i]);
                    yield return rest;
                }
            }
        }
    }
}
