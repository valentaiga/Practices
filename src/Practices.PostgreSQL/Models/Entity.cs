namespace Practices.PostgreSQL.Models;

public class Entity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
