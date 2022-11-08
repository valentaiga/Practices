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

    public async Task<IReadOnlyList<GameMatch>> GetMatches(int year, MatchRating rating)
    {
        await _tableCreator.CreateIfNotExist();
        var fetched = await _matchRepository.MatchesFetched(year, (int)rating);
        if (fetched)
        {
            var from = new DateTime(year, 1, 1);
            var to = new DateTime(year + 1, 1, 1).AddDays(-1);
            var result = await _matchRepository.GetMatches(from, to);
            return result.Matches;
        }

        var matches = await ScrapMatches(year, rating);
        await SaveMatches(matches);
        await _matchRepository.AddMatchesFetch(year, (int)rating);
        return matches;
    }

    private async Task<IReadOnlyList<GameMatch>> ScrapMatches(int year, MatchRating rating)
    {
        var from = new DateTime(year, 1, 1);
        var to = new DateTime(year + 1, 1, 1).AddDays(-1);
        var result = await _hltvWebClient.GetMatches(from, to, rating);
        return result;
    }

    private async Task SaveMatches(IReadOnlyList<GameMatch> matches)
    {
        foreach (var match in matches)
        {
            await _matchRepository.AddIfNotExists(match);
        }
    }
}

public interface IMatchService
{
    Task<IReadOnlyList<GameMatch>> GetMatches(int year, MatchRating rating);
}