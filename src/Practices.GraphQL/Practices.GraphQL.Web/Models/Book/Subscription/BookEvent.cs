namespace Practices.GraphQL.Web.Models.Book.Subscription;

public sealed class BookEvent
{
    public Book? Book { get; set; }
    public DateTime Timestamp { get; set; }
}