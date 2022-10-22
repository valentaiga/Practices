using GraphQL.Types;
using Practices.GraphQL.Models.Book.Subscription;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models;

public sealed class StoreSubscription : ObjectGraphType
{
    public StoreSubscription(IBookEventService eventService)
    {
        Field<BookEventType>("bookUpdated")
            .Description("Subscribe on book update")
            .ResolveStream(_ => eventService.OnUpdate());
    }
}