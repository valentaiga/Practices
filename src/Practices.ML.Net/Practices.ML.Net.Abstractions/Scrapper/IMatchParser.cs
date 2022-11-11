using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Abstractions.Scrapper;

public interface IMatchParser
{
    int ParseMatchId(string matchUrl);
    GameMatch ParseMatch(Stream stream, int matchId);
    string[] ParseMatches(Stream stream);
}