using System.Text.Json;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Options;
using Practices.GraphQL.GraphQL.Models;
using Practices.GraphQL.Options;

namespace Practices.GraphQL.Middleware;

public class GraphQLMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IGraphQLSerializer _writer;
    private readonly IDocumentExecuter _executor;
    private readonly GraphQLOptions _options;

    public GraphQLMiddleware(RequestDelegate next, IGraphQLSerializer writer, IDocumentExecuter executor, IOptions<GraphQLOptions> options)
    {
        _next = next;
        _writer = writer;
        _executor = executor;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext httpContext, ISchema schema)
    {
        if (httpContext.Request.Path.StartsWithSegments(_options.Endpoint) 
            && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
        {
            var request = await JsonSerializer
                .DeserializeAsync<GraphQLQuery>(
                    httpContext.Request.Body,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var result = await _executor
                .ExecuteAsync(doc =>
                {
                    doc.Schema = schema;
                    doc.Query = request.Query;
                }).ConfigureAwait(false);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 200;

            await _writer.WriteAsync(httpContext.Response.Body, result);
        }
        else
        {
            await _next(httpContext);
        }
    }

}