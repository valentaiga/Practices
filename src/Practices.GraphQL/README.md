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

### Schema
There are two ways you can build your schema: **Schema first approach** using the GraphQL schema language AND the other one is **GraphType** or **Code first approach** by writing GraphType classes.

### Schema First Approach
Using GraphQLMetadata to customize the mapping to the schema type.

### GraphType First Approach
The GraphType first approach gives you access to all of the provided properties of your GraphType's and Schema.

### Complexity Analyzer
Complexity analyzer prevents malicious queries.

### Data loader
A DataLoader helps in two ways - **improves data fetching** and **ensures consistency**:
- Similar operations are batched together. This can make fetching data over a network much more efficient.
- Fetched values are cached so if they are requested again, the cached value is returned.
 
### Executer (GraphQL .Net)
DocumentExecuter is a class which executes, validates, analyses the request by specified providers: ISchema, IDocumentBuilder, IDocumentValidator.  

### Subscriptions
Subscription helps to track any changes by asynchronous data stream.  
Rough data pipeline on sub: Event stream => Executer => Resolver => Response stream

## Conclusion
- GraphQL is evolving at rapid pace
- More control over data
  - Error boundaries => data quality
  - @defer/@stream => prioritisation
- Stronger type system

## Environment setup
Run the project, Altair UI is on https://localhost:5001/ui/altair  
Available schemas on right side of a screen.

Supported features:
- [x] Query single book by id parameter
- [x] Query all books
- [x] Mutation book create
- [x] Variables support
- [x] Organized schema
```
query {
  book {
    books {
      id
      description
      author {
        name
      }   
    }
  }
  author {
    authors {
      id
      name
      books {
        id
        name
      }
    }
  }
}
```
- [ ] Mutate single book (change its fields by id)
- [x] Author's books in model (and book's author)
- [ ] Subscription with book's title update event

## Project setup
Nothing to declare