using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Abstractions.Predictor;

public interface IMatchPredictor
{
    MatchPrediction Predict(
        IEnumerable<GameMatch> trainMatches,
        GameMatch matchToPredict,
        CancellationToken ct);

    MatchPrediction Predict(
        IEnumerable<GameMatch> trainMatches,
        GameMatch matchToPredict);
}