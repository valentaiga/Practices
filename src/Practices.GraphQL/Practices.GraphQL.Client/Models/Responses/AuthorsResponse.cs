namespace Practices.GraphQL.Client.Models.Responses;

public class AuthorsResponse
{
    public ICollection<AuthorData> Authors { get; set; }
}