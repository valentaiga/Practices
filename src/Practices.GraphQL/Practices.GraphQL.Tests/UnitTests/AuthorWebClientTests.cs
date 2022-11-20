namespace Practices.GraphQL.Tests.UnitTests;

[Collection("GraphQL Collection")]
public class AuthorWebClientTests
{
    private readonly IAuthorWebClient _client;

    public AuthorWebClientTests(GraphQLFixture fixture)
    {
        _client = fixture.GetRequiredServer<IAuthorWebClient>();
    }
    
    [Fact]
    public async Task Get_SingleAuthor_ReturnsAuthor()
    {
        var author = await _client.GetAuthor(AssertExtensions.ExistingId);
        
        Assert.Equal(AssertExtensions.ExistingId, author.Id);
        author.EnsureAuthorIsValid();
        Assert.All(author.Books, AssertExtensions.EnsureBookIsValid);
    }
    
    [Fact]
    public async Task Get_NotExistingAuthor_ThrowsError()
    {
        var getAuthorTask = _client.GetAuthor(AssertExtensions.NotExistingId);
        
        await Assert.ThrowsAsync<GraphQLException>(() => getAuthorTask);
    }
    
    [Fact]
    public async Task Get_AllAuthors_ReturnsAllAuthors()
    {
        var authors = await _client.GetAllAuthors();
        
        Assert.All(authors, author =>
        {
            author.EnsureAuthorIsValid();
            Assert.All(author.Books, AssertExtensions.EnsureBookIsValid);
        });
    }
    
    [Fact]
    public async Task Delete_CreatedAuthor_ReturnsSuccess()
    {
        var createReq = new CreateAuthorRequest("Test06");
        var author = await _client.CreateAuthor(createReq);

        await _client.DeleteAuthor(author.Id);
        var getAuthorTask = _client.GetAuthor(author.Id);
        await Assert.ThrowsAsync<GraphQLException>(() => getAuthorTask);
    }
    
    [Fact]
    public async Task Delete_NotExistingAuthor_ThrowsError()
    {
        var deleteAuthorTask = _client.DeleteAuthor(AssertExtensions.NotExistingId);
        
        await Assert.ThrowsAsync<GraphQLException>(() => deleteAuthorTask);
    }
    
    [Fact]
    public async Task Create_SingleAuthor_ReturnsAuthor()
    {
        const string name = "Test03";
        var request = new CreateAuthorRequest(name);
        var author = await _client.CreateAuthor(request);
        Assert.Equal(name, author.Name);
        author.EnsureAuthorIsValid();

        var getAuthor = await _client.GetAuthor(author.Id);
        Assert.Equal(name, getAuthor.Name);
    }
    
    [Fact]
    public async Task Update_ExistingAuthor_ReturnsAuthor()
    {
        const string name = "Test04_changed";
        var request = new UpdateAuthorRequest(AssertExtensions.ExistingId, name);
        var author = await _client.UpdateAuthor(request);
        
        Assert.Equal(name, author.Name);
        author.EnsureAuthorIsValid();
        Assert.All(author.Books, AssertExtensions.EnsureBookIsValid);

        var getAuthor = await _client.GetAuthor(author.Id);
        Assert.Equal(name, getAuthor.Name);
    }
    
    [Fact]
    public async Task Update_NotExistingAuthor_ThrowsError()
    {
        const string name = "Test07_changed";
        var request = new UpdateAuthorRequest(AssertExtensions.NotExistingId, name);
        var updateAuthorTask = _client.UpdateAuthor(request);
        
        await Assert.ThrowsAsync<GraphQLException>(() => updateAuthorTask);
    }
}