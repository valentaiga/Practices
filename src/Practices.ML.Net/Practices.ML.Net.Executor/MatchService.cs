using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Abstractions.Predictor;
using Practices.ML.Net.Abstractions.Repository;
using Practices.ML.Net.Abstractions.Scrapper;

namespace Practices.ML.Net.Executor;

public class MatchService : IMatchService
{
    private readonly IMatchWebClient _hltvWebClient;
    private readonly IMatchRepository _matchRepository;
    private readonly ITableCreator _tableCreator;
    private readonly IMatchPredictor _matchPredictor;
    private readonly ILogger<MatchService> _logger;

    public MatchService(
        IMatchWebClient hltvWebClient,
        IMatchRepository matchRepository,
        ITableCreator tableCreator,
        IMatchPredictor matchPredictor,
        ILogger<MatchService> logger)
    {
        _hltvWebClient = hltvWebClient;
        _matchRepository = matchRepository;
        _tableCreator = tableCreator;
        _matchPredictor = matchPredictor;
        _logger = logger;
    }

    public async Task CheckProbability()
    {
        const string matchUrl =
            "https://www.hltv.org/matches/2353171/vitality-vs-ninjas-in-pyjamas-iem-winter-2021";
            // "https://www.hltv.org/matches/2360065/ex-finest-vs-honoris-european-development-championship-6";
            // "https://www.hltv.org/matches/2353975/natus-vincere-vs-astralis-blast-premier-spring-groups-2022";
        var match = await _hltvWebClient.GetMatchInfo(matchUrl, throwOnParseError: true);
        
        var matches = await _matchRepository.GetMatches(
            new DateTime(2020, 1, 1), 
            new DateTime(2022, 1, 1))
            .ToArrayAsync();

        var prob = _matchPredictor.Predict(matches, match);
        return;
        
        // var splitDt = new DateTime(2021, 6, 1);
        // var trainData = matches.Where(x => x.Date < splitDt);
        // var testData = matches.Where(x => x.Date > splitDt);
        //
        // var prediction = _matchPredictor.Predict(trainData, testData, CancellationToken.None);
    }

    public async IAsyncEnumerable<GameMatch> GetMatches(int year, MatchRating rating)
    {
        await _tableCreator.CreateIfNotExist();
        var fetched = await _matchRepository.MatchesFetched(year, rating);
        if (fetched)
        {
            _logger.LogDebug("Matches for {year} with {rating} rating found in db.", year, rating);
            var from = new DateTime(year, 1, 1);
            var to = new DateTime(year + 1, 1, 1).AddDays(-1);
            await foreach (var match in _matchRepository.GetMatches(from, to))
            {
                yield return match;
            }

            _logger.LogDebug("Matches for {year} with {rating} returned from db", year, rating);
            yield break;
        }

        _logger.LogDebug("Matches for {year} with {rating} rating not found in db. Starting fetching.", year, rating);
        await foreach (var match in ScrapMatches(year, rating))
        {
            await _matchRepository.AddIfNotExists(match);
            yield return match;
        }
        _logger.LogDebug("Matches for {year} with {rating} rating returned from scraper.", year, rating);
        await _matchRepository.AddMatchesFetch(year, rating);
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
    Task CheckProbability();
}