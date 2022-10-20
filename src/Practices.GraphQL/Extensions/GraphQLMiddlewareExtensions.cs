using GraphQL;
using Practices.GraphQL.GraphQL;
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
            .AddGraphTypes()
            .AddSchema<StoreSchema>()
            .AddComplexityAnalyzer(opt =>
            {
#if !DEBUG
                opt.MaxComplexity = 200;
#endif
            })
        );

        return services.Configure(action);
    }
}