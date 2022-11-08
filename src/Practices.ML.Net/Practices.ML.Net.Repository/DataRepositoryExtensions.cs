using Practices.ML.Net.Repository.Services;

namespace Practices.ML.Net.Repository;

public static class DataRepositoryExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddSingleton<IInternalRepository, InternalRepository>();
        services.AddSingleton<IMatchRepository, MatchRepository>();
        services.AddSingleton<ITableCreator, TableCreator>();
        return services;
    }
}