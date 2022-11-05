using Data.Repository.Models;

namespace Data.Repository.Results;

public record MatchResult(
    int Id,
    int TournamentPrisePool,
    IEnumerable<int> Team1,
    IEnumerable<int> Team2,
    bool IsTeam1Win,
    DateTime Date);