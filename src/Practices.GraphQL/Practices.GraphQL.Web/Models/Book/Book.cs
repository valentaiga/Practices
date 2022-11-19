namespace Practices.GraphQL.Web.Models.Book;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    
#pragma warning disable CS8618
    private Book()
    {
    }
#pragma warning restore CS8618

    public Book(int id, string title, string description, int authorId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        AuthorId = authorId;
        CreatedAt = createdAt;
    }
}