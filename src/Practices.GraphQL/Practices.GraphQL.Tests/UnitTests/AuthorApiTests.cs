using Practices.GraphQL.Client.Models.Responses.Author;

namespace Practices.GraphQL.Tests.UnitTests;

[Collection("GraphQL Collection")]
public class AuthorApiTests
{
    private readonly GraphQLHttpClient _client;
    public AuthorApiTests(GraphQLFixture fixture)
    {
        _client = fixture.CreateGraphQLClient();
    }

    [Fact]
    public async Task Get_SingleAuthor_ReturnsSingleWithBooks()
    {
        const string query = @"query ($id:Int!){
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
        var request = new GraphQLHttpRequest(
          query, 
          variables: new { id = AssertExtensions.ExistingId });
        var response = await _client.SendQueryAsync<AuthorWrap<AuthorResponse>>(request);

        Assert.Null(response.Errors);
        var author = response.Data.Author?.Author;
        Assert.NotNull(author);
        author!.EnsureAuthorIsValid();
        Assert.All(author!.Books, AssertExtensions.EnsureBookIsValid);
    }

    [Fact]
    public async Task Get_AllAuthors_ReturnsManyWithBooks()
    {
        const string query = @"query {
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
        
        var request = new GraphQLHttpRequest(query);
        var response = await _client.SendQueryAsync<AuthorWrap<AuthorsResponse>>(request);

        Assert.Null(response.Errors);
        var authors = response.Data.Author?.Authors;
        Assert.NotNull(authors);
        foreach (var author in authors!)
        {
            author!.EnsureAuthorIsValid();
            Assert.All(author!.Books, AssertExtensions.EnsureBookIsValid);
        }
    }

    [Fact]
    public async Task Create_SingleAuthor_AuthorCreates()
    {
        var query = @"mutation ($name:String!){
  author {
    create(author: { name: $name }) {
      id
      name
      createdAt
    }
  }
}";
        const string name = "Aksel Sentoca";
        var request = new GraphQLHttpRequest(
          query, 
          variables: new { name = name });
        var response = await _client.SendQueryAsync<AuthorWrap<CreateAuthorResponse>>(request);

        Assert.Null(response.Errors);
        var author = response.Data.Author?.Create;
        author!.EnsureAuthorIsValid();
        Assert.Equal(name, author!.Name);
    }

    [Fact]
    public async Task Update_SingleAuthor_NameUpdates()
    { 
      var query = @"mutation ($id:Int!, $name:String!){
  author {
    update(author: { id: $id, name: $name }) {
      id
      name
      createdAt
    }
  }
}"; 
        const string newName = "Ivan Ukss";
        var request = new GraphQLHttpRequest(
          query, 
          variables: new { 
            id = AssertExtensions.ExistingId, 
            name = newName
          });
        var response = await _client.SendQueryAsync<AuthorWrap<UpdateAuthorResponse>>(request);

        Assert.Null(response.Errors);
        var author = response.Data.Author?.Update;
        author!.EnsureAuthorIsValid();
        Assert.Equal(newName, author!.Name);
    }
    
    [Fact]
    public async Task Delete_ExistingAuthor_Success()
    { 
        var query = @"mutation ($id:Int!){
  author {
    delete(id: $id)
  }
}";
        var request = new GraphQLHttpRequest(
            query, 
            variables: new { id = AssertExtensions.ExistingIdForDelete });
        var response = await _client.SendQueryAsync<AuthorWrap<DeleteAuthorResponse>>(request);

        Assert.Null(response.Errors);
        var success = response.Data.Author.Delete;
        Assert.True(success);
    }
    
    [Fact]
    public async Task Delete_NotExistingAuthor_Failure()
    { 
      var query = @"mutation ($id:Int!){
  author {
    delete(id: $id)
  }
}";
      var request = new GraphQLHttpRequest(
        query, 
        variables: new { id = AssertExtensions.ExistingIdForDelete });
      var response = await _client.SendQueryAsync<AuthorWrap<DeleteAuthorResponse>>(request);

      Assert.NotEmpty(response.Errors);
    }
}