using Practices.GraphQL.GraphQL.Book;

namespace Practices.GraphQL.Services;

public class BookRepository : IBookRepository
{
    // since its graphQL only practice, I would simplify logic a bit
    private static int _lastId = 3;
    private static readonly List<Book> Storage = new()
    {
        new Book(1, "History of Germany I", "Historical Book about Germany. Tome I", DateTime.UtcNow.AddYears(-5)),
        new Book(2, "History of Germany II", "Historical Book about Germany. Tome II", DateTime.UtcNow.AddDays(-3)),
        new Book(3, "History of Germany III", "Historical Book about Germany. Tome III", DateTime.UtcNow.AddDays(-1)),
    };
    
    public Task<Book?> Get(int id)
    {
        var result = Storage.Find(x => x.Id == id);
        return Task.FromResult(result);
    }

    public Task<List<Book>> GetAll()
    {
        return Task.FromResult(Storage);
    }

    public Task<Book> Create(string title, string description)
    {
        var book = new Book(Interlocked.Increment(ref _lastId), title, description, DateTime.UtcNow);
        Storage.Add(book);
        return Task.FromResult(book);
    }
}

public interface IBookRepository
{
    Task<Book?> Get(int id);
    Task<List<Book>> GetAll();
    Task<Book> Create(string title, string description);
}