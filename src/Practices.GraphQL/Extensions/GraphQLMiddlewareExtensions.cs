using GraphQL;
using Practices.GraphQL.GraphQL.Book;
using Practices.GraphQL.GraphQL.Book.Mutation;
using Practices.GraphQL.GraphQL.Book.Query;
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
        
        services.AddTransient<BookQuery>();
        services.AddTransient<BookType>();
        services.AddTransient<BookMutation>();
        services.AddTransient<BookInputType>();
        
        return services.Configure(action);
    }
}