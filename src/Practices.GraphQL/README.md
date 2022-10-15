# GraphQL 
Project includes WebApi with GraphQL over it

## Goals
 - [x] Build GraphQL project and understand the meaning of mandatory GraphQL over .Net classes

## Theory
### GraphQL is a query language for API
Operation  | GraphQL     | Rest
  ---- |-------------| ------------- 
Read  | Query       | GET
Write  | Mutation    | POST, PUT, PATCH, DELETE
Events  | Subscrition | N/A

- **HTTP POST** most common for **Query** and **Mutation**
- **HTTP GET** often used for **Query** when using persisted queries
- **WebSockets** most common for **Subscribtion**
- **gRPC** used in some instances

### Pros
- One endpoint, single request,
- No over/under fetching
- Strong type system gives very predictable to use the API from user perspective
- GraphQL isnt tied to any specific database/storage engine

### Cons:
- Multiple subscriptions => Multiplexing (WS protocol by GraphQL solved this problem)
- Throttling => Batching (Mobile/web overflow by events)

### Schema build
There are two ways you can build your schema: **Schema first approach** using the GraphQL schema language AND the other one is **GraphType** or **Code first approach** by writing GraphType classes.

### Schema First Approach
Use the optional GraphQLMetadata attribute to customize the mapping to the schema type.
```csharp
public class Query
{
  [GraphQLMetadata("hero")]
  public Droid GetHero()
  {
    return new Droid { Id = "1", Name = "R2-D2" };
  }
}

var json = await schema.ExecuteAsync(_ =>
{
  _.Query = "{ hero { id name } }";
});

>>> OUTPUT
{
  "data": {
    "hero": {
      "id": "1",
      "name": "R2-D2"
    }
  }
}
```
### GraphType First Approach
  The GraphType first approach gives you access to all of the provided properties of your GraphType's and Schema.
```csharp
public class DroidType : ObjectGraphType<Droid>
{
  public DroidType()
  {
    Field(x => x.Id).Description("The Id of the Droid.");
    Field(x => x.Name).Description("The name of the Droid.");
  }
}

public class StarWarsQuery : ObjectGraphType
{
  public StarWarsQuery()
  {
    Field<DroidType>("hero")
        .Resolve(context => new Droid { Id = "1", Name = "R2-D2" });
  }
}

var schema = new Schema { Query = new StarWarsQuery() };

var json = await schema.ExecuteAsync(_ =>
{
  _.Query = "{ hero { id name } }";
});

>>> OUTPUT
{
  "data": {
    "hero": {
      "id": "1",
      "name": "R2-D2"
    }
  }
}
```

### Data loader improves Data Fetching and also Ensures Consistency
On request **Data Loader** tries to get data from **Task Cache** if it is in here, otherwise **Data Loader** fetches data from source and stores it in cache.    
Data Loader could help in **GraphQL over old REST API** case, when many requests are waiting for each other and these requests can be parallelized and fetched in single response. 
 
### Executor (Subscriptions)
In cases when consumer wants a data stream, **Executor** could help  
Event stream => Executor => Resolver => Response stream
```csharp
[ExtendObjectType(OperationTypeNames.Subscription)]
public sealed class EntitySubscription 
{
    public async IAsyncEnumerable<int> CreateEntityChangeStream(
        [Service] IEntityChangeService service, // <-- server which tells us 'which entity changed'
        [EnumeratorCancellation] CancellationToken ct)
    {
        await foreach(var id in service.ReadAsync(ct))
            yield return id;
    }
    
    [Subscription(with=nameof(CreateEntityChangeStream))] // <-- subscribes to stream
    public async Task<Entity> OnEntityChangeAsync(
        EntityByIdDataLoader dataLoader,
        [EventMessage] int id,
        CancellationToken ct)
    => await dataLoader.LoadAsync(id, ct); // <-- Helps to load data efficiently
    
    // !!! Add `.AddSubscriptionType()` to GraphQL in Startup.cs
}
```
## Conclusion
- GraphQL is evolving at rapid pace
- More control over data
  - Error boundaries => data quality
  - @defer/@stream => prioritisation
- Stronger type system

## Environment setup

## Project setup
