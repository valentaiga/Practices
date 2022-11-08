using Microsoft.ML.Data;

namespace Practices.ML.Net.Predictor.Models;

public class MLPrediction
{
    [ColumnName(nameof(IsTeam1Win))] public bool IsTeam1Win { get; set; }
}