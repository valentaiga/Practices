using Practices.GraphQL.Client.Models.Responses.Book;

namespace Practices.GraphQL.Client.Models.Responses.Author;

public class AuthorData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<BookData> Books { get; set; }
}