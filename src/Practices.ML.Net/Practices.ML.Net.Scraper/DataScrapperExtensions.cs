using Practices.ML.Net.Scraper.Services;

namespace Practices.ML.Net.Scraper;

public static class DataScrapperExtensions
{
    public static IServiceCollection AddDataScrapper(this IServiceCollection services)
    {
        services.AddSingleton<IHltvParser, HltvParser>();
        services.AddSingleton<IHltvWebClient, HltvWebClient>();
        return services;
    }
}