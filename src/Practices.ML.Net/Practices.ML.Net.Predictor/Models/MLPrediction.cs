using Microsoft.ML.Data;

namespace Practices.ML.Net.Predictor.Models;

public class MLPrediction
{
    [ColumnName(nameof(MLMatch.Team1Win))] public bool Team1Win { get; set; }
    public float Score { get; set; }
}

public class MLMapPrediction
{
    [ColumnName(nameof(MLMatch.T1WinMaps))][KeyType(3)] public uint MapsWin { get; set; }
    public float Probability => (float)MapsWin / 3;
    public float Score { get; set; }
}