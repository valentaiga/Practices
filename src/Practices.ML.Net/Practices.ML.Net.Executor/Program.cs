﻿using Practices.ML.Net.Executor;
using Practices.ML.Net.Predictor;
using Practices.ML.Net.Repository;
using Practices.ML.Net.Scraper;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDataScrapper();
        services.AddRepository();
        services.AddMLServices();
        services.AddSingleton<IMatchService, MatchService>();
        services.AddLogging(builder => builder
            .AddConsole()
            .AddSystemdConsole()
            .SetMinimumLevel(LogLevel.Debug));
    });

var app = builder.Build();

try
{
    // just test the code
    var service = app.Services.GetRequiredService<IMatchService>();

    await service.CheckProbability();
    // foreach (var rating in Enum.GetValues<MatchRating>().Reverse())
    // {
    //     await service.GetMatches(2022, rating).ToArrayAsync();
    // }
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
