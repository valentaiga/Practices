using Dapper;
using Npgsql;
using Practices.ML.Net.Abstractions.Settings;

namespace Practices.ML.Net.Repository.Services;

public class TableCreator : ITableCreator
{
    private readonly string _connectionString;
    private readonly SemaphoreSlim _locker;
    private bool _created;

    public TableCreator()
    {
        _locker = new(1, 1);
        _connectionString = GlobalSettings.PostgresConnectionString;
    }

    public async Task CreateIfNotExist()
    {
        await _locker.WaitAsync();
        if (_created) return;
        await CreateMatchesTable();
        await CreateFetchesTable();
        _created = true;
        _locker.Release();
    }

    private async Task CreateMatchesTable()
    {
        if (await TableExists("matches"))
            return;

        const string matchesCreate = @"
create table matches (
    id INT PRIMARY KEY,
    tournament INT,
    date Date,
    t1 INT,
    t2 INT,
    t1_score INT,
    t2_score INT,
    player1 INT,
    player2 INT,
    player3 INT,
    player4 INT,
    player5 INT,
    player6 INT,
    player7 INT,
    player8 INT,
    player9 INT,
    player10 INT
)";
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(matchesCreate);
    }

    private async Task CreateFetchesTable()
    {
        if (await TableExists("fetches"))
            return;

        const string createFetches = @"
create table fetches (
    year INT PRIMARY KEY,
    rating SMALLINT
)";
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(createFetches);
    }

    private async Task<bool> TableExists(string tableName)
    {
        var q = @"
select exists (
    select * from pg_tables
    where schemaname = 'public' 
      and tablename  = @tableName
    limit 1
    )";
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleAsync<bool>(q, new { tableName });
    }
}

public interface ITableCreator
{
    Task CreateIfNotExist();
}