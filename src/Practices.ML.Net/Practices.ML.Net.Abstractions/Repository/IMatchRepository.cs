using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Abstractions.Repository;

public interface IMatchRepository
{
    IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to);
    Task AddIfNotExists(GameMatch gameMatch);
    Task<bool> IsMatchExists(int id);
    Task<bool> MatchesFetched(int year, MatchRating rating);
    Task AddMatchesFetch(int year, MatchRating rating);
}