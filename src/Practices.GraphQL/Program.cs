using Microsoft.AspNetCore;
using Practices.GraphQL.Extensions;
using Practices.GraphQL.Services;

namespace Practices.GraphQL;

public static class Program
{
    private static readonly CancellationTokenSource TokenSource = new();
    public static async Task Main(string[] args)
    {
        try
        {
            var app = CreateWebHostBuilder(args).Build();
            await app.RunAsync(TokenSource.Token);
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
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        => WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<IBookRepository, BookRepository>();
                services.AddSingleton<IAuthorRepository, AuthorRepository>();

                services.AddGraphQL(options =>
                {
                    options.Endpoint = "/graphql";
                });
            })
            .Configure(app =>
            {
                app.UseGraphQL();
            });
}