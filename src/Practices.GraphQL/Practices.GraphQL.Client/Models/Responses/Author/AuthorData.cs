using Practices.GraphQL.Client.Models.Responses.Book;

namespace Practices.GraphQL.Client.Models.Responses.Author;

public class AuthorData : AuthorShortData
{
    public ICollection<BookData> Books { get; set; }
}