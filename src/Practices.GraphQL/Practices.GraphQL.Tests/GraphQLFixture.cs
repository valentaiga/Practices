using Practices.GraphQL.Web;

namespace Practices.GraphQL.Tests;

public class GraphQLFixture : IDisposable
{
    private readonly TestServer _server;

    public GraphQLFixture()
    {
        var builder = Program.CreateWebHostBuilder(new string[] { })
            .ConfigureServices(services =>
            {
                services.AddScoped<IAuthorWebClient>(
                    _ => new GraphQLWebClient(
                        Path.Combine(_server.BaseAddress.AbsoluteUri, "graphql"), 
                        _server.CreateClient()));
            });
        _server = new TestServer(builder);
    }

    public TRequiredServer GetRequiredServer<TRequiredServer>() where TRequiredServer : notnull =>
        _server.Services.GetRequiredService<TRequiredServer>();

    public IAuthorWebClient GetAuthorWebClient() =>
        null;

    public GraphQLHttpClient CreateGraphQLClient()
    {
        var url = Path.Combine(_server.BaseAddress.AbsoluteUri, "graphql");
        var options = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(url)
        };
        var serializer = new SystemTextJsonSerializer();
        var httpClient = _server.CreateClient();
        return new GraphQLHttpClient(options, serializer, httpClient);
    }

    public void Dispose()
    {
        _server.Dispose();
    }
}