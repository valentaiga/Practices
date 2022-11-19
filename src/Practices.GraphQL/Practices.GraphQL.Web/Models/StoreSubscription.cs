using GraphQL.Types;
using Practices.GraphQL.Web.Models.Book.Subscription;
using Practices.GraphQL.Web.Services;

namespace Practices.GraphQL.Web.Models;

public sealed class StoreSubscription : ObjectGraphType
{
    public StoreSubscription(IBookEventService eventService)
    {
        Field<BookEventType>("bookUpdated")
            .Description("Subscribe on book update")
            .ResolveStream(_ => eventService.OnUpdate());
    }
}