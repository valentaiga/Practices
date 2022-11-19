using System.Net.WebSockets;
using System.Text.Json;
using GraphQL;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.WebSockets;
using GraphQL.Server.Transports.AspNetCore.WebSockets.SubscriptionsTransportWs;
using GraphQL.Types;
using Microsoft.Extensions.Options;
using Practices.GraphQL.Web.Models;
using Practices.GraphQL.Web.Options;

namespace Practices.GraphQL.Web.Middleware;

public class GraphQLMiddleware : IUserContextBuilder
{
    private readonly RequestDelegate _next;
    private readonly IGraphQLSerializer _writer;
    private readonly IDocumentExecuter _executor;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly GraphQLOptions _options;

    public GraphQLMiddleware(
        RequestDelegate next, 
        IGraphQLSerializer writer, 
        IDocumentExecuter executor,
        IHostApplicationLifetime hostApplicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<GraphQLOptions> options)
    {
        _next = next;
        _writer = writer;
        _executor = executor;
        _hostApplicationLifetime = hostApplicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext httpContext, ISchema schema)
    {
        if (httpContext.Request.Path.StartsWithSegments(_options.Endpoint))
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                await HandleWebSocket(httpContext);
                return;
            }

            if (string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                await HandleQueryRequest(httpContext, schema);
                return;
            }
        }
        
        await _next(httpContext);
    }

    private async Task HandleQueryRequest(HttpContext httpContext, ISchema schema)
    {
        var ct = httpContext.RequestAborted;
        var request = await JsonSerializer
            .DeserializeAsync<GraphQLQuery>(
                httpContext.Request.Body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, ct);

        var result = await _executor
            .ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Query = request!.Query;
                options.Variables = request.Variables;
                options.OperationName = request.OperationName;
                options.RequestServices = httpContext.RequestServices;
                options.ThrowOnUnhandledException = true;
                options.CancellationToken = ct;
            }).ConfigureAwait(false);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 200;

        await _writer.WriteAsync(httpContext.Response.Body, result, ct);
    }
    
    private async Task HandleWebSocket(HttpContext httpContext)
    {
        var subProtocol = httpContext.WebSockets.WebSocketRequestedProtocols.FirstOrDefault(x => _options.SupportedSocketProtocols.Contains(x));
        if (subProtocol is null)
        {
            await _next(httpContext);
            return;
        }
        
        var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync(subProtocol);

        if (webSocket.SubProtocol != subProtocol)
        {
            await webSocket.CloseAsync(
                WebSocketCloseStatus.ProtocolError,
                $"Invalid sub-protocol; expected '{subProtocol}'",
                httpContext.RequestAborted);
            return;
        }

        // Connect, then wait until the websocket has disconnected (and all subscriptions ended)
        var appStoppingToken = _hostApplicationLifetime.ApplicationStopping;
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(httpContext.RequestAborted, appStoppingToken);
        if (cts.Token.IsCancellationRequested)
            return;
        try
        {
            using var webSocketConnection = new WebSocketConnection(httpContext, webSocket, _writer, _options.WebSockets, cts.Token);
            using var messageProcessor = CreateMessageProcessor(webSocketConnection);
            await webSocketConnection.ExecuteAsync(messageProcessor);
        }
        catch (OperationCanceledException) when (appStoppingToken.IsCancellationRequested)
        {
            // terminate all pending WebSockets connections when the application is in the process of stopping

            // note: we are consuming OCE in this case because ASP.NET Core does not consider the task as canceled when
            // an OCE occurs that is not due to httpContext.RequestAborted; so to prevent ASP.NET Core from considering
            // this a "regular" exception, we consume it here
        }
    }

    private IOperationMessageProcessor CreateMessageProcessor(IWebSocketConnection webSocketConnection)
    {
        var authService = webSocketConnection.HttpContext.RequestServices.GetService<IWebSocketAuthenticationService>();

        return new SubscriptionServer(
            webSocketConnection,
            _options.WebSockets,
            _options,
            _executor,
            _writer,
            _serviceScopeFactory,
            this,
            authService);
    }

    public virtual async ValueTask<IDictionary<string, object?>?> BuildUserContextAsync(HttpContext context, object? payload)
    {
        // required for socket server
        var userContextBuilder = context.RequestServices.GetService<IUserContextBuilder>();
        return userContextBuilder == null
            ? null
            : await userContextBuilder.BuildUserContextAsync(context, payload);
    }
}