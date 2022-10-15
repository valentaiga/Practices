using Dapper;
using Microsoft.Data.SqlClient;
using Practices.PostgreSQL.Models;

namespace Practices.PostgreSQL;

public class PostgresRepository : IPostgresRepository
{
    private readonly PostgresSettings _settings; 
    
    public PostgresRepository(PostgresSettings settings)
    {
        _settings = settings;
    }

    public async Task<int> Create(Entity entity)
    {
        const string query = @"
insert into entities 
values (@title, @description)
RETURNING id";
        await using var conn = new SqlConnection(_settings.ConnectionString);
        return await conn.ExecuteScalarAsync<int>(query, entity);
    }

    public async Task Delete(int id)
    {
        const string query = @"
delete from entities 
where id = @id";
        await using var conn = new SqlConnection(_settings.ConnectionString);
        await conn.ExecuteAsync(query, id);
    }

    public async Task<Entity> Get(int id)
    {
        const string query = @"
select * from entities where id = @id";
        await using var conn = new SqlConnection(_settings.ConnectionString);
        return await conn.QuerySingleAsync<Entity>(query, id);
    }

    public async Task<IEnumerable<Entity>> GetAll(int limit = 1000, int offset = 0)
    {
        const string query = @"
select * from entities
limit @limit
offset @offset";
        await using var conn = new SqlConnection(_settings.ConnectionString);
        return await conn.QueryAsync<Entity>(query, new {offset, limit});
    }
}