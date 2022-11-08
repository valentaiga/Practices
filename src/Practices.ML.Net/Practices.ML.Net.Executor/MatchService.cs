using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Repository.Services;
using Practices.ML.Net.Scraper.Models;
using Practices.ML.Net.Scraper.Services;

namespace Practices.ML.Net.Executor;

public class MatchService : IMatchService
{
    private readonly IHltvWebClient _hltvWebClient;
    private readonly IMatchRepository _matchRepository;
    private readonly ITableCreator _tableCreator;

    public MatchService(
        IHltvWebClient hltvWebClient,
        IMatchRepository matchRepository,
        ITableCreator tableCreator)
    {
        _hltvWebClient = hltvWebClient;
        _matchRepository = matchRepository;
        _tableCreator = tableCreator;
    }

    public async IAsyncEnumerable<GameMatch> GetMatches(int year, MatchRating rating)
    {
        await _tableCreator.CreateIfNotExist();
        var fetched = await _matchRepository.MatchesFetched(year, (int)rating);
        if (fetched)
        {
            var from = new DateTime(year, 1, 1);
            var to = new DateTime(year + 1, 1, 1).AddDays(-1);
            await foreach (var match in _matchRepository.GetMatches(from, to))
            {
                yield return match;
            }

            yield break;
        }

        await foreach (var match in ScrapMatches(year, rating))
        {
            await _matchRepository.AddIfNotExists(match);
            yield return match;
        }
        await _matchRepository.AddMatchesFetch(year, (int)rating);
    }

    private async IAsyncEnumerable<GameMatch> ScrapMatches(int year, MatchRating rating)
    {
        var from = new DateTime(year, 1, 1);
        var to = new DateTime(year + 1, 1, 1).AddDays(-1);
        await foreach (var match in _hltvWebClient.GetMatches(from, to, rating))
        {
            yield return match;
        }
    }
}

public interface IMatchService
{
    IAsyncEnumerable<GameMatch> GetMatches(int year, MatchRating rating);
}