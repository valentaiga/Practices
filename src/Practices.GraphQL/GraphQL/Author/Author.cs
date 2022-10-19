namespace Practices.GraphQL.GraphQL.Author;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

#pragma warning disable CS8618
    private Author()
    {
    }
#pragma warning restore CS8618

    public Author(int id, string name, DateTime createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
    }
}