using Data.Repository.Services;

namespace Data.Repository;

public static class DataRepositoryExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddSingleton<IInternalMatchRepository, InternalMatchRepository>();
        services.AddSingleton<IMatchRepository, MatchRepository>();
        return services;
    }
}