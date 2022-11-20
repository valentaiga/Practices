using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Practices.GraphQL.Client.Models;
using static Practices.GraphQL.Client.Models.OperationResult;

namespace Practices.GraphQL.Client;

public abstract class GraphQLWebClientBase: IDisposable
{
    private readonly GraphQLHttpClient _httpClient;

    protected GraphQLWebClientBase(string graphQLUrl, HttpClient httpClient)
    {
        var url = new Uri(graphQLUrl);
        var serializer = new SystemTextJsonSerializer(); 
        _httpClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = url,
                WebSocketEndPoint = url
            }, 
            serializer, 
            httpClient);
    }
    
    protected async Task<OperationResult<TData>> RequestAsync<TResponseType, TData>(
        string query,
        object variables,
        Func<TResponseType, TData> map,
        CancellationToken ct)
    {
        GraphQLResponse<TResponseType> response;
        try
        {
            var request = new GraphQLHttpRequest(
                query, 
                variables: variables);
            response = await _httpClient.SendQueryAsync<TResponseType>(request, ct);

        }
        catch (Exception ex)
        {
            return Fail<TData>(GraphQLException.FromException(ex));
        }

        if (response.Errors is null)
            return Success(map.Invoke(response.Data));

        var exception = GraphQLException.FromResponse(response);
        return Fail<TData>(exception);
    }
    
    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}