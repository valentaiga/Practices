namespace Practices.GraphQL.Client.Constants;

public static class AuthorQueries
{
    public const string GetById = @"query ($id:Int!){
  author {
    author(id: $id) {
      id
      name
      createdAt
      books {
        id
        description
        title
        createdAt
      }
    }
  }
}";

    public const string GetAll = @"query {
  author {
    authors {
      id
      name
      createdAt
      books {
        id
        description
        title
        createdAt
      }
    }
  }
}";

    public const string Create = @"mutation ($name:String!){
  author {
    create(author: { name: $name }) {
      id
      name
      createdAt
    }
  }
}";

    public const string Update = @"mutation ($id:Int!, $name:String!){
  author {
    update(author: { id: $id, name: $name }) {
      id
      name
      createdAt
    }
  }
}";

    public const string Delete = @"mutation ($id:Int!){
  author {
    delete(id: $id)
  }
}";
}