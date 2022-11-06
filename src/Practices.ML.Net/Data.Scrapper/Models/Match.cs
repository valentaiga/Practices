namespace Data.Scrapper.Models;

public record Match(
    int Id,
    int Tournament,
    DateTime Date,
    int T1,
    int T2,
    int T1Score,
    int T2Score,
    int[] T1Players,
    int[] T2Players);