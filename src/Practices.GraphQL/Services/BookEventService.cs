using System.Reactive.Linq;
using System.Reactive.Subjects;
using Practices.GraphQL.Models.Book;
using Practices.GraphQL.Models.Book.Subscription;

namespace Practices.GraphQL.Services;

public class BookEventService : IBookEventService
{
    private readonly ISubject<BookEvent> _events = new ReplaySubject<BookEvent>();

    public void BookChanged(Book book)
    {
        var bookEvent = new BookEvent
        {
            Book = book,
            Timestamp = DateTime.UtcNow
        };
        _events.OnNext(bookEvent);
    }
        
    public IObservable<BookEvent> OnUpdate()
    {
        return _events.AsObservable();
    }
}

public interface IBookEventService
{
    void BookChanged(Book book);
    IObservable<BookEvent> OnUpdate();
}