using Practices.GraphQL.Extensions;
using Practices.GraphQL.Services;

namespace Practices.GraphQL;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.ConfigureServices();
        
        var app = builder.Build();
        app.UseGraphQL();
        await app.RunAsync();
    }
    
    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBookRepository, BookRepository>();
        
        builder.Services.AddGraphQL(options =>
        {
            options.Endpoint = "/graphql";
        });
    }
}