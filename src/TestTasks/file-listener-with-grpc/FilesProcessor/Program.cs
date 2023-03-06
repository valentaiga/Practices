using Microsoft.AspNetCore;

try
{
    var app = CreateWebHostBuilder(args).Build();
    await app.RunAsync();
}
catch (TaskCanceledException)
{
    // ignore
}
catch (Exception ex)
{
    // log error
    Console.WriteLine($"Unable to start. Exception: {ex}");
}

static IWebHostBuilder CreateWebHostBuilder(string[] args)
    => WebHost.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            
        });
        
