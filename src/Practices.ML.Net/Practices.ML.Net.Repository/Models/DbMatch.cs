namespace Practices.ML.Net.Repository.Models;

internal class DbMatch
{
    public int Id { get; init; }
    public int Tournament { get; init; }
    public DateTime Date { get; init; }
    public int T1 { get; init; }
    public int T2 { get; init; }
    public int T1Score { get; init; }
    public int T2Score { get; init; }
    public int Player1 { get; init; }
    public int Player2 { get; init; }
    public int Player3 { get; init; }
    public int Player4 { get; init; }
    public int Player5 { get; init; }
    public int Player6 { get; init; }
    public int Player7 { get; init; }
    public int Player8 { get; init; }
    public int Player9 { get; init; }
    public int Player10 { get; init; }

    public DbMatch()
    {
    }

    public DbMatch(int id, int tournament, DateTime date, int t1, int t2, int t1Score, int t2Score, int player1, int player2, int player3, int player4, int player5, int player6, int player7, int player8, int player9, int player10)
    {
        Id = id;
        Tournament = tournament;
        Date = date;
        T1 = t1;
        T2 = t2;
        T1Score = t1Score;
        T2Score = t2Score;
        Player1 = player1;
        Player2 = player2;
        Player3 = player3;
        Player4 = player4;
        Player5 = player5;
        Player6 = player6;
        Player7 = player7;
        Player8 = player8;
        Player9 = player9;
        Player10 = player10;
    }
}