using Dapper;
using Data.Repository.Models;
using Microsoft.Data.SqlClient;

namespace Data.Repository.Services;

internal class InternalMatchRepository : IInternalMatchRepository
{
    private readonly string _connectionString;

    public InternalMatchRepository()
    {
        _connectionString = "";
    }


    public async Task<IEnumerable<Match>> GetMatches(DateTime from, DateTime to)
    {
        const string q = @"
select *
from matches 
where date > @from 
    and date < @to
order by date asc";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<Match>(q, new { from, to });
    }

    public async Task AddMatch(Match match)
    {
        const string q = @"
insert into matches
values (@id, 
    @TournamentPrisePool,
    @Date,
    @IsTeam1Win,
    @Player1,
    @Player2,
    @Player3,
    @Player4,
    @Player5,
    @Player6,
    @Player7,
    @Player8,
    @Player9,
    @Player10)";
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(q, match);
    }
}

internal interface IInternalMatchRepository
{
    Task<IEnumerable<Match>> GetMatches(DateTime from, DateTime to);
    Task AddMatch(Match match);
}