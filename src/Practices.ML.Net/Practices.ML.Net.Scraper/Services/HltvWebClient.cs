using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Scraper.Models;

namespace Practices.ML.Net.Scraper.Services;

internal class HltvWebClient : WebClientBase, IHltvWebClient
{
    private readonly IHltvParser _parser;

    public HltvWebClient(IHltvParser parser)
    {
        _parser = parser;
    }
    
    public async IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to, MatchRating rating)
    {
        await foreach (var url in GetMatchInfoUrls(from, to, rating))
        {
            var parseResult = await GetMatchInfo(url);
            yield return parseResult;
            Console.WriteLine($"Match ({parseResult.Id} {parseResult.T1} vs {parseResult.T2}) fetched");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    private async Task<GameMatch> GetMatchInfo(string matchUrl)
    {
        await using var stream = await HttpClient.GetStreamAsync(matchUrl);
        var matchId = _parser.ParseMatchId(matchUrl);
        try
        {
            return _parser.ParseMatch(stream, matchId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return GameMatch.FromError(matchId);
        }
    }

    private async IAsyncEnumerable<string> GetMatchInfoUrls(DateTime from, DateTime to, MatchRating rating)
    {
        var relativeUrl = string.Empty;
        var offset = 0;

        while (true)
        {
            UpdateUrl();
            var response = await HttpClient.GetAsync(relativeUrl);
            EnsureSuccessResponse(response);
            await using var stream = await response.Content.ReadAsStreamAsync();
            var parseResult = _parser.ParseMatches(stream);
            foreach (var url in parseResult)
            {
                yield return url;
            }
            offset += parseResult.Length;
            Console.WriteLine($"Fetched '{relativeUrl}'");
            if (parseResult.Length != 100)
                break;
            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        void UpdateUrl() => 
            relativeUrl = $"/results?startDate={from:yyyy-MM-dd}&endDate={to:yyyy-MM-dd}&stars={(int)rating}&offset={offset}";
    }
}

public interface IHltvWebClient
{
    IAsyncEnumerable<GameMatch> GetMatches(DateTime from, DateTime to, MatchRating rating);
}