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
    
    public async Task<IReadOnlyList<GameMatch>> GetMatches(DateTime from, DateTime to, MatchRating rating)
    {
        var result = new List<GameMatch>(100);
        
        var matchUrls = await GetMatchInfoUrls(from, to, rating);
        foreach (var url in matchUrls)
        {
            var parseResult = await GetMatchInfo(url);
            result.Add(parseResult);
            Console.WriteLine($"Match ({parseResult.Id} {parseResult.T1} vs {parseResult.T2}) fetched");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
        
        return result;
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

    private async Task<List<string>> GetMatchInfoUrls(DateTime from, DateTime to, MatchRating rating)
    {
        var relativeUrl = string.Empty;
        var offset = 0;

        var results = new List<string>(100);

        var finishFetch = false;

        while (!finishFetch)
        {
            UpdateUrl();
            var response = await HttpClient.GetAsync(relativeUrl);
            EnsureSuccessResponse(response);
            await using var stream = await response.Content.ReadAsStreamAsync(); 
            var parseResult = _parser.ParseMatches(stream);
            results.AddRange(parseResult);
            offset += parseResult.Length;
            finishFetch = parseResult.Length != 100;
            Console.WriteLine($"Fetched '{relativeUrl}'");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        Console.WriteLine($"Matches fetched Total:{results.Count}");

        return results;
        
        void UpdateUrl() => 
            relativeUrl = $"/results?startDate={from:yyyy-MM-dd}&endDate={to:yyyy-MM-dd}&stars={(int)rating}&offset={offset}";
    }
}

public interface IHltvWebClient
{
    Task<IReadOnlyList<GameMatch>> GetMatches(DateTime from, DateTime to, MatchRating rating);
}