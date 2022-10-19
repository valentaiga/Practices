using Practices.GraphQL.GraphQL.Book;

namespace Practices.GraphQL.Services;

public class BookRepository : IBookRepository
{
    // since its graphQL only practice, I simplified logic
    private static int _lastId = 3;
    private static readonly List<Book> Storage = new()
    {
        new Book(1, "History of Germany I", "Historical Book about Germany. Tome I",  1, DateTime.UtcNow.AddYears(-5)),
        new Book(2, "History of Germany II", "Historical Book about Germany. Tome II", 1, DateTime.UtcNow.AddDays(-3)),
        new Book(3, "History of Germany III", "Historical Book about Germany. Tome III", 1, DateTime.UtcNow.AddDays(-1)),
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

    public Task<Book> Create(string title, string description, int authorId)
    {
        var book = new Book(Interlocked.Increment(ref _lastId), title, description, authorId, DateTime.UtcNow);
        Storage.Add(book);
        return Task.FromResult(book);
    }

    public Task Delete(int id)
    {
        var book = Storage.Find(x => x.Id == id);
        if (book is not null)
            Storage.Remove(book);
        return Task.CompletedTask;
    }
}

public interface IBookRepository
{
    Task<Book?> Get(int id);
    Task<List<Book>> GetAll();
    Task<Book> Create(string title, string description, int authorId);
    Task Delete(int id);
}