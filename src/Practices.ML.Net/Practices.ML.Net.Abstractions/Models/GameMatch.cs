namespace Practices.ML.Net.Abstractions.Models;

public record GameMatch(
    int Id,
    int Tournament,
    DateTime Date,
    int T1,
    int T2,
    int T1Score,
    int T2Score,
    int[] T1Players,
    int[] T2Players)
{
    public static GameMatch FromError(int id)
        => new GameMatch(id, 0, DateTime.MinValue, 0, 0, 0, 0, new[] { 0, 0, 0, 0, 0 }, new[] { 0, 0, 0, 0, 0 });
}