using Practices.ML.Net.Abstractions.Scrapper;
using Practices.ML.Net.Scraper.Services;

namespace Practices.ML.Net.Scraper;

public static class DataScrapperExtensions
{
    public static IServiceCollection AddDataScrapper(this IServiceCollection services)
    {
        services.AddSingleton<IMatchParser, HltvParser>();
        services.AddSingleton<IMatchWebClient, HltvWebClient>();
        return services;
    }
}