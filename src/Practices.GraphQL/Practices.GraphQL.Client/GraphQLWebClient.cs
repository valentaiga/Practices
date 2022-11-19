using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace Practices.GraphQL.Client;

public class GraphQLWebClient : IDisposable
{
    public string EndpointUrl { get; internal set; } = "localhost";

    private readonly GraphQLHttpClient _client;
    public GraphQLWebClient()
    {
        var serializer = new SystemTextJsonSerializer();
        _client = new GraphQLHttpClient(EndpointUrl, serializer);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}