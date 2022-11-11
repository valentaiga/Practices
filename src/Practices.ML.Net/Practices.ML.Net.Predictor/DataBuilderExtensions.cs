using Practices.ML.Net.Abstractions.Predictor;
using Practices.ML.Net.Predictor.Services;

namespace Practices.ML.Net.Predictor;

public static class DataBuilderExtensions
{
    public static IServiceCollection AddMLServices(this IServiceCollection services)
    {
        services.AddSingleton<IMatchPredictor, MatchPredictor>();
        services.AddSingleton<IDataTransformer, DataTransformer>();
        // todo: configure ML services
        return services;
    }
}