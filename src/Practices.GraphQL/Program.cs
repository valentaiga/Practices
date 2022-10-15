using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Practices.GraphQL.Extensions;
using Practices.GraphQL.GraphQL.Models;
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
        // no need before subscription support realization
        // app.UseWebSockets();
        // app.MapControllers();
        app.UseRouting();
        await app.RunAsync();
    }
    
    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBookRepository, BookRepository>();
        
        // builder.Services.AddControllers();
        builder.Services.AddGraphQL(options =>
        {
            options.Endpoint = "/graphql";
        });


        // todo: check if unnecessary
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
        // builder.Services.AddGraphQL(b => b
        //     .AddSchema<BookSchema>()
        //     .AddAutoSchema<GraphQLQuery>()
        //     .AddSystemTextJson()
        //     .AddGraphTypes(typeof(BookType).Assembly));
    }
}