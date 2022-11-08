using Practices.ML.Net.Executor;
using Practices.ML.Net.Predictor;
using Practices.ML.Net.Repository;
using Practices.ML.Net.Scraper;
using Practices.ML.Net.Scraper.Models;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDataScrapper();
        services.AddRepository();
        services.AddMLServices();
        services.AddSingleton<IMatchService, MatchService>();
    });

var app = builder.Build();

try
{
    // just test the code
    var service = app.Services.GetRequiredService<IMatchService>();
    foreach (var rating in Enum.GetValues<MatchRating>().Reverse())
    {
        var matches = await service.GetMatches(2020, rating).ToArrayAsync();
        Console.WriteLine($"fetched {matches.Length} matches ({(int)rating} rating)");
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
