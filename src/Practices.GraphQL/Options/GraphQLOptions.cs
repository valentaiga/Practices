using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.WebSockets;

namespace Practices.GraphQL.Options;

public class GraphQLOptions : IAuthorizationOptions
{
    public string? Endpoint { get; set; }
    public bool SubscribesEnabled { get; set; }
    public GraphQLWebSocketOptions WebSockets { get; set; } = new();
    public bool AuthorizationRequired { get; }
    public IEnumerable<string> AuthorizedRoles { get; } = Enumerable.Empty<string>();
    public string? AuthorizedPolicy { get; }

    public readonly IReadOnlyList<string> SupportedSocketProtocols
        = new[]
        {
            "graphql-transport-ws",
            "graphql-ws"
        };
}