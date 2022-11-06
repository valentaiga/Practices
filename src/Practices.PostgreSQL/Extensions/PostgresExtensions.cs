using Microsoft.Extensions.DependencyInjection;

namespace Practices.PostgreSQL.Extensions;

public static class PostgresExtensions
{
    public static void ConfigurePostgres(IServiceCollection services, Action<PostgresSettings> action)
    {
        var settings = new PostgresSettings();
        action(settings);
        services.AddSingleton<PostgresSettings>(_ => settings);
        services.AddSingleton<IPostgresRepository, PostgresRepository>();
    }
}