using GraphQL.Types;

namespace Practices.GraphQL.Web.Models;

public sealed class StoreSchema : Schema
{
    public StoreSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<StoreQuery>();
        Mutation = serviceProvider.GetRequiredService<StoreMutation>();
        Subscription = serviceProvider.GetRequiredService<StoreSubscription>();
    }
}