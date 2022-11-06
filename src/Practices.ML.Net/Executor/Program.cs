using Data.Builder;
using Data.Repository;
using Data.Scrapper;
using Executor;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDataScrapper();
        services.AddRepository();
        services.AddMLServices();
        services.AddSingleton<ICommandExecutor, CommandExecutor>();
    });

var app = builder.Build();

try
{
    // Executor just runs the needed code to ensure that it works
    var executor = app.Services.GetRequiredService<ICommandExecutor>();
    await executor.ScrapData();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
