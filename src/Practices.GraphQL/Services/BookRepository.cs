using Practices.GraphQL.GraphQL.Models;

namespace Practices.GraphQL.Services;

public class BookRepository : IBookRepository
{
    // todo: split to api/dbo models
    private static List<Book> Storage = new()
    {
        new Book(1, "Hystory of Germany I", "Hystorical Book about Germany. Tome I", DateTime.UtcNow.AddYears(-5)),
        new Book(2, "Hystory of Germany II", "Hystorical Book about Germany. Tome II", DateTime.UtcNow.AddDays(-3)),
        new Book(3, "Hystory of Germany III", "Hystorical Book about Germany. Tome III", DateTime.UtcNow.AddDays(-1)),
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
}

public interface IBookRepository
{
    Task<Book?> Get(int id);
    Task<List<Book>> GetAll();
}