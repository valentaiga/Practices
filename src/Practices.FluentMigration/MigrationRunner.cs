using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Practices.FluentMigration.MIgrations;
using Practices.FluentMigration.Models;

namespace Practices.FluentMigration;

public class MigrationRunner
{
    // In case DI/Migrations/etc in different project, migrate extension may be more cool solution
    // public static IApplicationBuilder Migrate(this IApplicationBuilder app)
    // {
    //     using var scope = app.ApplicationServices.CreateScope();
    //     var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
    //     runner.ListMigrations();
    //     runner.MigrateUp(20200601100000);
    //     return app;
    // }
    
    public void Run(string connectionString, MigrationType mt)
    {
        var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider, mt);
    }
    
    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                // !!! I would use main project config to get connection string instead (but its practice, right?)
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreateTableEntities).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider, MigrationType mt)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        if (mt == MigrationType.Up)
            runner.MigrateUp();
        else
            runner.MigrateDown(0);
    }
}