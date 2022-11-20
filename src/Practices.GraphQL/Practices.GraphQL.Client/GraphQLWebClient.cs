using Practices.GraphQL.Client.Constants;
using Practices.GraphQL.Client.Models.Requests.Author;
using Practices.GraphQL.Client.Models.Responses.Author;

namespace Practices.GraphQL.Client;

public class GraphQLWebClient : GraphQLWebClientBase, IAuthorWebClient
{
    public GraphQLWebClient(string graphQLUrl, HttpClient httpClient)
        : base(graphQLUrl, httpClient)
    {
    }

    public async Task<AuthorData> GetAuthor(int id, CancellationToken ct = default) =>
        await Query(
            AuthorQueries.GetById,
            new { id = id },
            (AuthorWrap<AuthorResponse> response) => response.Author.Author,
            ct);

    public async Task<ICollection<AuthorData>> GetAllAuthors(CancellationToken ct = default) =>
        await Query(
            AuthorQueries.GetAll,
            null,
            (AuthorWrap<AuthorsResponse> response) => response.Author.Authors,
            ct);

    public async Task DeleteAuthor(int id, CancellationToken ct = default) =>
        await Query(
            AuthorQueries.Delete,
            new { id = id },
            (AuthorWrap<DeleteAuthorResponse> response) => response.Author.Delete,
            ct);
    
    public async Task<AuthorShortData> CreateAuthor(CreateAuthorRequest request, CancellationToken ct = default) =>
        await Query(
            AuthorQueries.Create,
            request,
            (AuthorWrap<CreateAuthorResponse> response) => response.Author.Create,
            ct);

    public async Task<AuthorData> UpdateAuthor(UpdateAuthorRequest request, CancellationToken ct = default) =>
        await Query(
            AuthorQueries.Update,
            request,
            (AuthorWrap<UpdateAuthorResponse> response) => response.Author.Update,
            ct);

    private async Task<TData> Query<TResponseType, TData>(
        string query,
        object variables,
        Func<TResponseType, TData> map,
        CancellationToken ct)
    {
        
        var result = await RequestAsync(
            query,
            variables,
            map,
            ct);

        return result.IsError
            ? throw result.Error
            : result.Result;
    }
}

public interface IAuthorWebClient
{
    Task<AuthorData> GetAuthor(int id, CancellationToken ct = default);
    Task<ICollection<AuthorData>> GetAllAuthors(CancellationToken ct = default);
    Task DeleteAuthor(int id, CancellationToken ct = default);
    Task<AuthorShortData> CreateAuthor(CreateAuthorRequest request, CancellationToken ct = default);
    Task<AuthorData> UpdateAuthor(UpdateAuthorRequest request, CancellationToken ct = default);
}