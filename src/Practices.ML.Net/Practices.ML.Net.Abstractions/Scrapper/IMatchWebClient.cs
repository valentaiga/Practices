using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Abstractions.Scrapper;

public interface IMatchWebClient
{
    IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to, MatchRating rating);
    Task<GameMatch> GetMatchInfo(string matchUrl, int? matchId = null, bool throwOnParseError = false);
}