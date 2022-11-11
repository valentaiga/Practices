using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Predictor.Models;

namespace Practices.ML.Net.Predictor.Services;

internal class DataTransformer : IDataTransformer
{
    public MLMatch Transform(GameMatch match)
        => ToML(match);

    private static MLMatch ToML(GameMatch m)
        => new()
        {
            // MatchId = m.Id,
            // TournamentPrisePool = 0,
            T1P = m.T1Players[0].ToString(),
            T2P = m.T1Players[1].ToString(),
            T3P = m.T1Players[2].ToString(),
            T4P = m.T1Players[3].ToString(),
            T5P = m.T1Players[4].ToString(),
            T6P = m.T2Players[0].ToString(),
            T7P = m.T2Players[1].ToString(),
            T8P = m.T2Players[2].ToString(),
            T9P = m.T2Players[3].ToString(),
            T10P = m.T2Players[4].ToString(),
            // WinProbability = m.T2Score == 0 ? 1 : (float)m.T1Score / m.T2Score,
            WinProbabilityT1 = (float)m.T1Score / (m.T1Score + m.T2Score),
            // WinProbabilityT2 = (float)m.T2Score / (m.T1Score + m.T2Score),
            IsPlayedIn6Months = m.Date.AddMonths(6) > DateTime.Today,
            IsPlayedIn3Months = m.Date.AddMonths(3) > DateTime.Today,
            Team1Win = m.T1Score > m.T2Score,
            MatchId = m.Id,
            MatchIdLong = (uint)m.Id,
            T1WinMaps = (uint)(m.T1Score / 16),
            VectorA = new []
            {
                (float)m.T1Score / (m.T1Score + m.T2Score),
                (float)m.T2Score / (m.T1Score + m.T2Score),
            },
            // RankT1 = 0,
            // RankT2 = 0
        };
}

internal interface IDataTransformer
{
    MLMatch Transform(GameMatch match);
}