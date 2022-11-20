namespace Practices.GraphQL.Client.Models.Responses.Author;

public class AuthorsResponse
{
    public ICollection<AuthorData> Authors { get; set; }
}