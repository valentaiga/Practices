using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.WebSockets;

namespace Practices.GraphQL.Web.Options;

public class GraphQLOptions : IAuthorizationOptions
{
    public string? Endpoint { get; internal set; } = null;
    public bool SubscribesEnabled { get; internal set; } = false;
    public GraphQLWebSocketOptions WebSockets { get; internal set; } = new();
    public bool AuthorizationRequired { get; internal set; } = false;
    public IEnumerable<string> AuthorizedRoles { get; internal set; } = Enumerable.Empty<string>();
    public string? AuthorizedPolicy { get; internal set; } = null;

    public readonly IReadOnlyList<string> SupportedSocketProtocols
        = new[]
        {
            "graphql-transport-ws",
            "graphql-ws"
        };
}