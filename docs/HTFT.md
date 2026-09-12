# Half-Time/Full-Time (HT/FT) Market

Predicts HT/FT combinations (e.g. Home HT → Home FT) via `HalfTimeFullTimePredictionService` (`ConsoleApp1/Implementations/HalfTimeFullTimePredictionService.cs`), wired into the daily prediction run in `ConsoleApp1/Program.cs`.

## Daily output

`Program.cs` calls `_htftService.AnalyseHTFT(homeTeam, awayTeam)` for each of the day's fixtures (`Program.cs:232`) and appends the result to `Data/Results/Predictions_{date}.txt` alongside the other markets.

## Backtest

`ConsoleApp1/Implementations/HTFTBacktester.cs` walks historical matches chronologically, retraining only on data before each match date to avoid lookahead, and computes win rate / EV per HT/FT combo using a hardcoded typical-odds table (`GetOddsForCombo`) rather than real market odds — treat its EV output as indicative, not exact.

To run it: set `bool RUN_HTFT_BACKTEST = true;` in `Program.cs` (currently the flag exists but the backtest call itself is commented out around `Program.cs:83-130` — uncomment before running). Output target: `HTFTBacktest_{date}.txt`.

## Interpreting results

- 🟢 65–75%+ win rate → profitable range worth acting on
- 🟡 55–65% → marginal, want more data before staking
- 🔴 <55% → not profitable as-is

Validate with a backtest before staking real money, and treat the backtester's odds as estimates until real market odds are wired in.
