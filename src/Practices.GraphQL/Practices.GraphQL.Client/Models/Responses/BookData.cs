namespace Practices.GraphQL.Client.Models.Responses;

public class BookData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public AuthorData Author { get; set; }
}