namespace Practices.GraphQL.Tests;

public static class AssertExtensions
{
    public static readonly DateTime DefaultDt = new();
    public const int DefaultInt = 0;
    public const int ExistingId = 1;
    public const int ExistingIdForDelete = 2;
    public const int NotExistingId = -1;

    public static void EnsureAuthorDtoIsValid(this Author author)
    {
        Assert.NotNull(author.Name);
        Assert.NotEqual(DefaultDt, author.CreatedAt);
        Assert.NotEqual(DefaultInt, author.Id);
    }

    public static void EnsureAuthorIsValid(this AuthorShortData author)
    {
        Assert.NotNull(author.Name);
        Assert.NotEqual(DefaultDt, author.CreatedAt);
        Assert.NotEqual(DefaultInt, author.Id);
    }

    public static void EnsureBookIsValid(this BookData book)
    {
        Assert.NotNull(book.Title);
        Assert.NotNull(book.Description);
        Assert.NotEqual(DefaultDt, book.CreatedAt);
        Assert.NotEqual(DefaultInt, book.Id);
    }
}