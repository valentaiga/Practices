using Microsoft.ML.Data;

namespace Data.Builder.Models;

public class MLMatch
{
    [LoadColumn(0)] public int TournamentPrisePool { get; set; }
    [LoadColumn(1, 5)] public int[] T1 { get; set; }
    [LoadColumn(6, 10)] public int[] T2 { get; set; }
    [LoadColumn(17)] public int RankT1 { get; set; }
    [LoadColumn(18)] public int RankT2 { get; set; }
    [LoadColumn(11)] public string Map { get; set; }
    [LoadColumn(12)] public int ScoreT1 { get; set; }
    [LoadColumn(13)] public int ScoreT2 { get; set; }
    [LoadColumn(14)] public bool IsPlayedIn6Months { get; set; }
    [LoadColumn(15)] public bool IsPlayedIn3Months { get; set; }
}