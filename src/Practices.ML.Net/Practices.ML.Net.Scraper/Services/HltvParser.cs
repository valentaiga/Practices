using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Scraper.Services;

internal class HltvParser : IHltvParser
{
    private static readonly Regex NumberRegex = new("\\d+");
    private static readonly Regex MatchIdRegex = new("\\d{7}");

    public int ParseMatchId(string matchUrl)
        => int.Parse(MatchIdRegex.Match(matchUrl).Value);

    public GameMatch ParseMatch(Stream stream, int matchId)
    {
        const string tournamentSelector = "/html/body/div/div/div/div/div/div/div/div[3]/a";
        const string dateSelector = "/html/body/div/div/div/div/div/div[2]/div[2]/div[1]";
        const string teamSelector = "/html/body/div/div/div/div/div/div/div/div/table/tr[1]/td/div/a";
        const string scoreSelector = "/html/body/div/div/div/div/div/div[2]/div/div/div";
        const string playerSelector = "/html/body/div/div/div/div/div/div/div/div/div/table/tr/td/a";

        try
        {
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
            var score = html.DocumentNode.SelectNodes(scoreSelector).Select(x => int.Parse(x.InnerText)).ToArray();
            var players = html.DocumentNode.SelectNodes(playerSelector)
                .Select(x => x.GetAttributeValue("href", null))
                .Where(x => x is not null)
                .Select(x => int.Parse(
                    NumberRegex.Match(x).Value))
                .Distinct().ToArray();

            return new GameMatch(
                matchId,
                tournament,
                date,
                teams[0],
                teams[1],
                score[0],
                score[1],
                players[..5],
                players[5..]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
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

internal interface IHltvParser
{
    int ParseMatchId(string matchUrl);
    GameMatch ParseMatch(Stream stream, int matchId);
    string[] ParseMatches(Stream stream);
}