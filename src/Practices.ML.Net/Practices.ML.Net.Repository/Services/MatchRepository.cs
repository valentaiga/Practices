using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Abstractions.Repository;
using Practices.ML.Net.Repository.Models;

namespace Practices.ML.Net.Repository.Services;

internal class MatchRepository : IMatchRepository
{
    private readonly IInternalRepository _repo;

    public MatchRepository(IInternalRepository repo)
    {
        _repo = repo;
    }

    public async Task AddIfNotExists(GameMatch gameMatch)
    {
        var exists = await _repo.MatchExists(gameMatch.Id);
        if (exists)
            return;
        var dbMatch = ToDb(gameMatch);
        await _repo.AddMatch(dbMatch);
    }

    public async IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to)
    {
        var dbMatches = await _repo.GetMatches(from, to);
        foreach (var dbMatch in dbMatches)
        {
            yield return ToModel(dbMatch);
        }
    }

    public Task<bool> IsMatchExists(int id)
        => _repo.MatchExists(id);

    public Task<bool> MatchesFetched(int year, MatchRating rating)
        => _repo.IsFetched(year, (int)rating);

    public Task AddMatchesFetch(int year, MatchRating rating)
        => _repo.AddMatchesFetch(year, (int)rating);
    
    private static DbMatch ToDb(GameMatch m)
    {
        var t1Players = m.T1Players.OrderBy(x => x).ToArray();
        var t2Players = m.T2Players.OrderBy(x => x).ToArray();
        return new DbMatch(
            m.Id,
            m.Tournament,
            m.Date,
            m.T1,
            m.T2,
            m.T1Score,
            m.T2Score,
            t1Players[0],
            t1Players[1],
            t1Players[2],
            t1Players[3],
            t1Players[4],
            t2Players[0],
            t2Players[1],
            t2Players[2],
            t2Players[3],
            t2Players[4]);
    }

    private static GameMatch ToModel(DbMatch m)
        => new GameMatch(
            m.Id,
            m.Tournament,
            m.Date,
            m.T1, 
            m.T2, 
            m.T1Score, 
            m.T2Score, 
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
            });
}