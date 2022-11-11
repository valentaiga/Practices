using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Abstractions.Repository;
using Practices.ML.Net.Abstractions.Scrapper;
using Practices.ML.Net.Abstractions.Settings;

namespace Practices.ML.Net.Scraper.Services;

internal class HltvWebClient : WebClientBase, IMatchWebClient
{
    private readonly IMatchParser _parser;
    private readonly IMatchRepository _matchRepository;
    private readonly ILogger<HltvWebClient> _logger;
    private readonly TimeSpan _requestCooldown;

    public HltvWebClient(
        IMatchParser parser,
        IMatchRepository matchRepository,
        ILogger<HltvWebClient> logger)
    {
        _parser = parser;
        _matchRepository = matchRepository;
        _logger = logger;
        _requestCooldown = GlobalSettings.RequestCooldown;
    }
    
    public async IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to, MatchRating rating)
    {
        await foreach (var matchUrl in GetMatchUrls(from, to, rating))
        {
            var matchId = _parser.ParseMatchId(matchUrl);
            var matchFetched = await _matchRepository.IsMatchExists(matchId);
            if (matchFetched)
                continue;
            var parseResult = await GetMatchInfo(matchUrl, matchId);
            yield return parseResult;
            
            await Task.Delay(_requestCooldown);
        }
    }

    public async Task<GameMatch> GetMatchInfo(string matchUrl, int? matchId = null, bool throwOnParseError = false)
    {
        matchId ??= _parser.ParseMatchId(matchUrl);
        var response = await HttpClient.GetAsync(matchUrl);
        EnsureSuccessResponse(response);
        await using var stream = await response.Content.ReadAsStreamAsync();
        
        _logger.LogDebug("Loaded match {matchId} web page {matchUrl}",matchId, GetFullUrl(matchUrl));
        try
        {
            var match = _parser.ParseMatch(stream, matchId.Value);
            _logger.LogDebug("Match {matchId} successfully parsed", matchId);
            return match;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Match {matchId} failed on parse", matchId);
            if (throwOnParseError)
                throw;
            return GameMatch.FromError(matchId.Value);
        }
    }

    private async IAsyncEnumerable<string> GetMatchUrls(DateTime from, DateTime to, MatchRating rating)
    {
        var offset = 0;

        while (true)
        {
            var relativeUrl = GetUrl();
            var response = await HttpClient.GetAsync(relativeUrl);
            EnsureSuccessResponse(response);
            _logger.LogDebug("Match urls on page {matchesUrl} successfully fetched", GetFullUrl(relativeUrl));
            
            await using var stream = await response.Content.ReadAsStreamAsync();
            var parseResult = _parser.ParseMatches(stream);
            _logger.LogDebug("Match urls on page {matchesUrl} successfully parsed", GetFullUrl(relativeUrl));
            
            foreach (var url in parseResult)
            {
                yield return url;
            }
            offset += parseResult.Length;
            
            if (parseResult.Length != 100)
                break;
            await Task.Delay(_requestCooldown);
        }
        
        _logger.LogDebug("Match urls for {year} with {rating} successfully parsed. {count} total", from.Year, rating, offset);

        string GetUrl() => 
            $"/results?startDate={from:yyyy-MM-dd}&endDate={to:yyyy-MM-dd}&stars={(int)rating}&offset={offset}";
    }
}