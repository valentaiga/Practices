using Grpc.Core;

namespace Practices.gRPC.Server.Services;

public class BooksClient : BookRepository.BookRepositoryBase
{
    private static readonly List<Book> Db = new()
    {
        GenBook(0),
        GenBook(1),
        GenBook(3)
    };

    private static Book GenBook(int id)
    {
        var authorId = id * 10 + id;
        return new Book
        {
            Id = id,
            Title = $"Title of book {id}",
            Description = $"Description of book {id}",
            AuthorId = authorId,
            AuthorName = $"{authorId} name"
        };
    }
    
    public override Task<Book> Get(GetBookRequest request, ServerCallContext context)
    {
        var book = Db.Find(x => x.Id == request.Id);
        if (book is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"), "Book not found");
        
        return Task.FromResult(book);
    }

    public override Task<Book> Create(CreateBookRequest request, ServerCallContext context)
    {
        var book = new Book
        {
            Id = Db.Count,
            Title = request.Title,
            Description = request.Description,
            AuthorId = request.AuthorId,
            AuthorName = request.AuthorId.ToString()
        };
        
        Db.Add(book);
        return Task.FromResult(book);
    }

    public override Task<DeleteBookResponse> Delete(DeleteBookRequest request, ServerCallContext context)
    {
        var book = Db.Find(x => x.Id == request.Id);
        if (book is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"), "Book not found");

        Db.Remove(book);
        var resp = new DeleteBookResponse
        {
            Success = true
        };
        return Task.FromResult(resp);
    }
}