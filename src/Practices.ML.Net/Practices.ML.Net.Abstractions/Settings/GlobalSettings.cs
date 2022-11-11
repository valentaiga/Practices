namespace Practices.ML.Net.Abstractions.Settings;

public static class GlobalSettings
{
    // todo: import with options from appSettings.json
    public const string PostgresConnectionString = "Host=localhost;Port=5432;User ID=postgres;Password=admin;Database=ml_db";
    public static TimeSpan RequestCooldown = TimeSpan.FromSeconds(2);
}