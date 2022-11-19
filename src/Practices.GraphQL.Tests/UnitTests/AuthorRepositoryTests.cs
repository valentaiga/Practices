namespace Practices.GraphQL.Tests.UnitTests;

[Collection("GraphQL Collection")]
public class AuthorRepositoryTests
{
    private readonly DateTime _defaultDt = new();
    private const int DefaultInt = 0;
    private const int ExistingId = 1;
    private const int NotExistingId = -1;

    private readonly IAuthorRepository _repo;

    public AuthorRepositoryTests(GraphQLFixture fixture)
    {
        _repo = fixture.GetRequiredServer<IAuthorRepository>();
    }

    [Fact]
    public async Task Create_SingleAuthor_ReturnsAuthor()
    {
        const string authorName = "Test01";
        var author = await _repo.Create(authorName);
        Assert.Equal(authorName, author.Name);
        EnsureAuthorIsCorrect(author);
    }

    [Fact]
    public async Task Get_SingleAuthor_ReturnsAuthor()
    {
        var author = await _repo.Get(ExistingId);
        Assert.NotNull(author);
    }

    [Fact]
    public async Task Get_AllAuthors_ReturnsAuthors()
    {
        var authors = await _repo.GetAll();
        Assert.All(authors, EnsureAuthorIsCorrect);
    }

    [Fact]
    public async Task Delete_CreatedAuthor_ReturnsTrue()
    {
        var author = await _repo.Create("Test02");
        await _repo.Delete(author.Id);
        var deletedAuthor = await _repo.Get(author.Id);
        Assert.Null(deletedAuthor);
    }

    [Fact]
    public async Task Update_CreatedAuthor_ReturnsAuthor()
    {
        const string updatedName = "new name";
        var author = await _repo.Create("Test03");
        var updatedAuthor = await _repo.Update(author.Id, a =>
        {
            a.Name = updatedName;
        });
        
        var getAuthor = await _repo.Get(author.Id);
        
        Assert.Equal(updatedName, updatedAuthor.Name);
        Assert.Equal(updatedName, getAuthor!.Name);
        EnsureAuthorIsCorrect(updatedAuthor);
    }

    [Fact]
    public async Task Exists_ExistingAuthor_ReturnsTrue()
    {
        var exists = await _repo.Exists(ExistingId);
        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_NotExistingAuthor_ReturnsFalse()
    {
        var exists = await _repo.Exists(NotExistingId);
        Assert.False(exists);
    }

    private void EnsureAuthorIsCorrect(Author author)
    {
        Assert.NotNull(author.Name);
        Assert.NotEqual(_defaultDt, author.CreatedAt);
        Assert.NotEqual(DefaultInt, author.Id);
    }
}