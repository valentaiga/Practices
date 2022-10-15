using GraphQL;
using Practices.GraphQL.GraphQL.Models;
using Practices.GraphQL.Middleware;
using Practices.GraphQL.Options;

namespace Practices.GraphQL.Extensions;

public static class GraphQLMiddlewareExtensions
{
    public static IApplicationBuilder UseGraphQL(this IApplicationBuilder builder)
    {
        builder.UseGraphQLAltair();
        return builder.UseMiddleware<GraphQLMiddleware>();
    }

    public static IServiceCollection AddGraphQL(this IServiceCollection services, Action<GraphQLOptions> action)
    {
        services.AddGraphQL(b => b
            .AddSystemTextJson()
            .AddDocumentExecuter<DocumentExecuter>()
            .AddSchema<BookSchema>());

        // builder.Services.AddSingleton<IDocumentWriter, DocumentWriter>();
        // builder.Services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
        // builder.Services.AddTransient<ISchema, BookSchema>();
        
        services.AddTransient<BookQuery>();
        services.AddTransient<BookType>();
        
        return services.Configure(action);
    }
}