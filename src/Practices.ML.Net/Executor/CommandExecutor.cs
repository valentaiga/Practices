using Data.Repository.Services;
using Data.Scrapper.Models;
using Data.Scrapper.Services;

namespace Executor;

public class CommandExecutor : ICommandExecutor
{
    private readonly IHltvWebClient _hltvWebClient;
    private readonly IMatchRepository _matchRepository;

    public CommandExecutor(
        IHltvWebClient hltvWebClient)
        // IMatchRepository matchRepository)
    {
        _hltvWebClient = hltvWebClient;
        _matchRepository = null;
    }

    public async Task ScrapData()
    {
        var from = new DateTime(2020, 1, 1);
        var to = new DateTime(2021, 1, 1).AddDays(-1);
        var data = await _hltvWebClient.GetMatches(from, to, MatchStars.Five);
        
        return;
    }
}

public interface ICommandExecutor
{
    Task ScrapData();
}