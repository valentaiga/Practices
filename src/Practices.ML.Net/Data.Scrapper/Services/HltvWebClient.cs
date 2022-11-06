using Data.Scrapper.Models;
using Match = Data.Scrapper.Models.Match;

namespace Data.Scrapper.Services;

internal class HltvWebClient : WebClientBase, IHltvWebClient
{
    private readonly IHltvParser _parser;

    public HltvWebClient(IHltvParser parser)
    {
        _parser = parser;
    }
    
    public async Task<IReadOnlyList<Match>> GetMatches(DateTime from, DateTime to, MatchStars stars)
    {
        var result = new List<Match>(100);
        
        var matchUrls = await GetMatchInfoUrls(from, to, stars);
        foreach (var url in matchUrls)
        {
            var parseResult = await GetMatchInfo(url);
            result.Add(parseResult);
            Console.WriteLine($"Match ({parseResult.Id} {parseResult.T1} vs {parseResult.T2}) fetched");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
        
        return result;
    }

    private async Task<Match> GetMatchInfo(string matchUrl)
    {
        await using var stream = await HttpClient.GetStreamAsync(matchUrl);
        var matchId = _parser.ParseMatchId(matchUrl);
        return _parser.ParseMatch(stream, matchId);
    }

    private async Task<List<string>> GetMatchInfoUrls(DateTime from, DateTime to, MatchStars stars)
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
            relativeUrl = $"/results?startDate={from:yyyy-MM-dd}&endDate={to:yyyy-MM-dd}&stars={(int)stars}&offset={offset}";
    }
}

public interface IHltvWebClient
{
    Task<IReadOnlyList<Match>> GetMatches(DateTime from, DateTime to, MatchStars stars);
}