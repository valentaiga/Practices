using Microsoft.ML.Data;

namespace Data.Builder.Models;

public class MLPrediction
{
    [ColumnName(nameof(IsTeam1Win))] public bool IsTeam1Win { get; set; }
}