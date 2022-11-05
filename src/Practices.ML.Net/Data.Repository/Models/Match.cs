namespace Data.Repository.Models;

internal record Match(
    int Id, 
    int TournamentPrisePool,
    DateTime Date,
    bool IsTeam1Win,
    int Player1,
    int Player2,
    int Player3,
    int Player4,
    int Player5,
    int Player6,
    int Player7,
    int Player8,
    int Player9,
    int Player10);