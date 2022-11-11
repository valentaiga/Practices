using Microsoft.ML.Data;

namespace Practices.ML.Net.Predictor.Models;

public class MLMatch
{
    [LoadColumn(0)] public bool Team1Win { get; set; }
    [LoadColumn(1)] public string T1P { get; set; }
    [LoadColumn(2)] public string T2P { get; set; }
    [LoadColumn(3)] public string T3P { get; set; }
    [LoadColumn(4)] public string T4P { get; set; }
    [LoadColumn(5)] public string T5P { get; set; }
    [LoadColumn(6)] public string T6P { get; set; }
    [LoadColumn(7)] public string T7P { get; set; }
    [LoadColumn(8)] public string T8P { get; set; }
    [LoadColumn(9)] public string T9P { get; set; }
    [LoadColumn(10)] public string T10P { get; set; }
    [LoadColumn(11)] public float WinProbabilityT1 { get; set; }
    [LoadColumn(12)] public bool IsPlayedIn6Months { get; set; }
    [LoadColumn(13)] public bool IsPlayedIn3Months { get; set; }
    [LoadColumn(14)] [VectorType(2)]public float[] VectorA { get; set; }
    [LoadColumn(15)] public int MatchId { get; set; }
    [KeyType(1_000_000)] public uint MatchIdLong { get; set; }
    [KeyType(3)] public uint T1WinMaps { get; set; }
}