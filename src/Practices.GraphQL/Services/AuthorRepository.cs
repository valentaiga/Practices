using Practices.GraphQL.Models.Author;

namespace Practices.GraphQL.Services;

public class AuthorRepository : IAuthorRepository
{
    // since its graphQL practice, I simplified logic
    private static int _lastId = 2;

    private static readonly List<Author> Storage = new()
    {
        new Author(1, "Meegan Norsky", DateTime.UtcNow.AddYears(-30)),
        new Author(2, "Big Lebovsky", DateTime.UtcNow.AddYears(-43)),
        new Author(3, "Anthony Jones", DateTime.UtcNow.AddYears(-67))
    };
    
    public Task<Author?> Get(int id)
    {
        var result = Storage.Find(x => x.Id == id);
        return Task.FromResult(result);
    }

    public Task<bool> Exists(int id)
    {
        var result = Storage.Exists(x => x.Id == id);
        return Task.FromResult(result);
    }

    public Task<List<Author>> GetAll()
    {
        return Task.FromResult(Storage);
    }

    public Task<Author> Create(string name)
    {
        var author = new Author(Interlocked.Increment(ref _lastId), name, DateTime.UtcNow);
        Storage.Add(author);
        return Task.FromResult(author);
    }

    public Task<Author> Update(int id, Action<Author> update)
    {
        var author = Storage.Find(x => x.Id == id);
        update.Invoke(author);
        return Task.FromResult(author);
    }

    public Task Delete(int id)
    {
        var author = Storage.Find(x => x.Id == id);
        if (author is not null)
            Storage.Remove(author);
        return Task.CompletedTask;
    }
}

public interface IAuthorRepository
{
    Task<Author?> Get(int id);
    Task<bool> Exists(int id);
    Task<List<Author>> GetAll();
    Task<Author> Create(string name);
    Task<Author> Update(int id, Action<Author> update);
    Task Delete(int id);
}