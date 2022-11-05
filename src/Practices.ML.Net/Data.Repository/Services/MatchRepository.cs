using Data.Repository.Models;
using Data.Repository.Results;

namespace Data.Repository.Services;

public class MatchRepository : IMatchRepository
{
    private readonly IInternalMatchRepository _repo;

    internal MatchRepository(IInternalMatchRepository repo)
    {
        _repo = repo;
    }

    public async Task Add(MatchResult match)
    {
        var m = ToDb(match);
        await _repo.AddMatch(m);
    }

    public async Task<GetMatchesResult> GetMatches(DateTime from, DateTime to)
    {
        var m = await _repo.GetMatches(from, to);
        var mResult = m.Select(ToResult).ToList();
        return new GetMatchesResult(mResult);
    }
    
    private static Match ToDb(MatchResult m)
    {
        var players = m.Team1.Union(m.Team2).ToArray();
        return new Match(
            m.Id,
            m.TournamentPrisePool,
            m.Date,
            m.IsTeam1Win,
            players[0],
            players[1],
            players[2],
            players[3],
            players[4],
            players[5],
            players[6],
            players[7],
            players[8],
            players[9]);
    }

    private static MatchResult ToResult(Match m)
        => new MatchResult(
            m.Id,
            m.TournamentPrisePool,
            new[]
            {
                m.Player1,
                m.Player2,
                m.Player3,
                m.Player4,
                m.Player5,
            },
            new[]
            {
                m.Player6,
                m.Player7,
                m.Player8,
                m.Player9,
                m.Player10,
            },
            m.IsTeam1Win,
            m.Date);
}

public interface IMatchRepository
{
    Task<GetMatchesResult> GetMatches(DateTime from, DateTime to);
    Task Add(MatchResult match);
}