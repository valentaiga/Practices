using Data.Scrapper.Services;

namespace Data.Scrapper;

public static class DataScrapperExtensions
{
    public static IServiceCollection AddDataScrapper(this IServiceCollection services)
    {
        services.AddSingleton<IHltvParser, HltvParser>();
        services.AddSingleton<IHltvWebClient, HltvWebClient>();
        return services;
    }
}