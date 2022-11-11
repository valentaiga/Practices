using System.Collections.Immutable;
using Dapper;
using Npgsql;
using Practices.ML.Net.Abstractions.Settings;
using Practices.ML.Net.Repository.Models;

namespace Practices.ML.Net.Repository.Services;

internal class InternalRepository : IInternalRepository
{
    private readonly ILogger<InternalRepository> _logger;
    private readonly string _connectionString;

    public InternalRepository(ILogger<InternalRepository> logger)
    {
        _logger = logger;
        _connectionString = GlobalSettings.PostgresConnectionString;
    }

    public async Task<ImmutableArray<DbMatch>> GetMatches(DateTime from, DateTime to)
    {
        const string q = @"
select *
from matches 
where date > @from 
    and date < @to
order by date asc";
        await using var conn = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(q, new { from, to }, flags: CommandFlags.NoCache);
        var result = await conn.QueryAsync<DbMatch>(command);
        return result.ToImmutableArray();
    }

    public async Task<bool> MatchExists(int matchId)
    {
        const string q = @"
select count(1) 
from matches 
where id = @matchId";
        await using var conn = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(q, new { matchId }, flags: CommandFlags.NoCache);
        return await conn.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddMatch(DbMatch dbMatch)
    {
        const string q = @"
insert into matches
values (@Id,
    @tournament,
    @date,
    @t1,
    @t2,
    @t1Score,
    @t2Score,
    @player1,
    @player2,
    @player3,
    @player4,
    @player5,
    @player6,
    @player7,
    @player8,
    @player9,
    @player10)";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(q, dbMatch);
        _logger.LogDebug("Match {matchId} successfully added to db", dbMatch.Id);
    }

    public async Task<bool> IsFetched(int year, int rating)
    {
        const string q = @"
select count(1) 
from fetches 
where year = @year 
    and rating = @rating";
        await using var conn = new NpgsqlConnection(_connectionString);
        var command = new CommandDefinition(q, new { year, rating }, flags: CommandFlags.NoCache);
        return await conn.ExecuteScalarAsync<bool>(command);
    }

    public async Task AddMatchesFetch(int year, int rating)
    {
        const string q = @"
insert into fetches
values (@year, @rating)";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.ExecuteAsync(q, new { year, rating });
        _logger.LogDebug("Matches fetch log for {year} with {rating} rating successfully added to db", year, rating);
    }
}

internal interface IInternalRepository
{
    Task<ImmutableArray<DbMatch>> GetMatches(DateTime from, DateTime to);
    Task AddMatch(DbMatch dbMatch);
    Task<bool> MatchExists(int matchId);
    Task<bool> IsFetched(int year, int rating);
    Task AddMatchesFetch(int year, int rating);
}