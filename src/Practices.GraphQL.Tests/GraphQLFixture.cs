namespace Practices.GraphQL.Tests;

public class GraphQLFixture : IDisposable
{
    private readonly TestServer _server;
    
    public GraphQLFixture()
    {
        _server = new TestServer(Program.CreateWebHostBuilder(new string[] { }));
    }

    public TRequiredServer GetRequiredServer<TRequiredServer>() where TRequiredServer : notnull 
        => _server.Services.GetRequiredService<TRequiredServer>();

    public void Dispose()
    {
        _server.Dispose();
    }
}