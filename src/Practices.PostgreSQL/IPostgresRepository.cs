using Practices.PostgreSQL.Models;

namespace Practices.PostgreSQL;

public interface IPostgresRepository
{
    Task<int> Create(Entity entity);
    Task Delete(int id);
    Task<Entity> Get(int id);
    Task<IEnumerable<Entity>> GetAll(int limit = 1000, int offset = 0);
}