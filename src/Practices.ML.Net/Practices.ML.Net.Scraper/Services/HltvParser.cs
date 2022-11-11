using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Abstractions.Scrapper;

namespace Practices.ML.Net.Scraper.Services;

internal class HltvParser : IMatchParser
{
    private static readonly Regex NumberRegex = new("\\d+");
    private static readonly Regex MatchIdRegex = new("\\d{7}");

    private readonly ILogger<HltvParser> _logger;

    public HltvParser(ILogger<HltvParser> logger)
    {
        _logger = logger;
    }

    public int ParseMatchId(string matchUrl)
        => int.Parse(MatchIdRegex.Match(matchUrl).Value);

    public GameMatch ParseMatch(Stream stream, int matchId)
    {
        const string tournamentSelector = "//div[@class='timeAndEvent']/div/a"; //"/html/body/div/div/div/div/div/div/div/div[3]/a";
        const string dateSelector = "//div[@class='timeAndEvent']/div[@class='date']"; //"/html/body/div/div/div/div/div/div[2]/div[2]/div[1]";
        const string teamSelector = "//div[@class='team']/div/a";
        const string scoreSelector = "//div[@class='mapholder']/div[2]/div[2]/div[2]";
        const string playerSelector = "//div[@class='lineups']/div/div/div/table/tr/td[@class='player']/div";
        const string playerSelect = "//div[@class='lineups']/div/div/div/table/tr/td[@class='player']/a";

        _logger.LogDebug("Match {matchId} parsing started", matchId);
        var html = new HtmlDocument();
        html.Load(stream);

        var tournament = int.Parse(
            NumberRegex.Match(
                html.DocumentNode
                    .SelectSingleNode(tournamentSelector)
                    .GetAttributeValue("href", null)).Value
        );
        var jsTicks = long.Parse(
            html.DocumentNode.SelectSingleNode(dateSelector).GetAttributeValue("data-unix", null));
        var date = FromJsTicksToDt(jsTicks);
        var teams = html.DocumentNode.SelectNodes(teamSelector)
            .Select(x =>
                int.Parse(
                    NumberRegex.Match(
                        x.GetAttributeValue("href", null)).Value))
            .Distinct().ToArray();
        // var score = html.DocumentNode.SelectNodes(scoreSelector).Select(x => int.Parse(x.InnerText)).ToArray();
        var scores = new[] { 0, 0 };
        foreach (var node in html.DocumentNode.SelectNodes(scoreSelector) ?? Enumerable.Empty<HtmlNode>()) // multiple maps per single match possible
        {
            var rounds = NumberRegex.Matches(node.InnerText).Select(x => x.Value).ToArray();
            for (var j = 0; j < rounds.Length; j++)
            {
                scores[j % 2] += int.Parse(rounds[j]);
            }
        }

        var players = html.DocumentNode.SelectNodes(playerSelector)
            ?.Select(x => int.Parse( 
                x.GetAttributeValue("data-player-id", null)))
            .ToArray()
        ?? html.DocumentNode.SelectNodes(playerSelect)
            .Select(x => int.Parse(
                NumberRegex.Match(
                    x.GetAttributeValue("href", null)).Value))
            .ToArray();

        _logger.LogDebug("All objects successfully parsed for match {matchId}", matchId);

        return new GameMatch(
            matchId,
            tournament,
            date,
            teams[0],
            teams[1],
            scores[0],
            scores[1],
            players[..5],
            players[5..]);
    }

    private static DateTime FromJsTicksToDt(long jsTicks)
    {
        const long dtInitValue = 621355968000000000;
        return new DateTime(jsTicks * 10000 + dtInitValue);
    }

    public string[] ParseMatches(Stream stream)
    {
        const string resultSelector =
            "/html/body/div/div/div/div/div/div/div/div/div/a";
        var html = new HtmlDocument();
        html.Load(stream);
        return html.DocumentNode.SelectNodes(resultSelector)
            .Select(x => x.GetAttributeValue("href", null)).ToArray();
    }
}