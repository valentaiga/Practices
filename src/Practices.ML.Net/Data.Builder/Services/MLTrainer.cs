using Data.Builder.Models;
using Microsoft.ML;
using Microsoft.ML.AutoML;

namespace Data.Builder.Services;

public class MLTrainer
{
    public MLTrainer()
    {
    }

    public async Task<bool> Predict(IEnumerable<MLMatch> matches, CancellationToken ct)
    {
        var mlContext = new MLContext(0);
        var dataSet = mlContext.Data.LoadFromEnumerable(matches);

        // var experimentSettings = new RegressionExperimentSettings()
        // {
        //     MaxExperimentTimeInSeconds = 60,
        //     CancellationToken = ct,
        //     CacheDirectoryName = null
        // };
        // var experimentSettings = new BinaryExperimentSettings();
        // var experimentSettings = new MulticlassExperimentSettings();
        IEstimator<ITransformer> trainingPipeline = 
            mlContext.Transforms.NormalizeMeanVariance(nameof(MLMatch.RankT1), "n_rankt1")
                .Append(mlContext.Transforms.NormalizeMeanVariance(nameof(MLMatch.RankT2), "n_rankt2"))
                .Append(mlContext.Transforms.NormalizeMeanVariance(nameof(MLMatch.ScoreT1), "n_scoret1"))
                .Append(mlContext.Transforms.NormalizeMeanVariance(nameof(MLMatch.ScoreT2), "n_scoret2"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(MLMatch.T1), "n_t1"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(MLMatch.T2), "n_t2"))
                .Append(mlContext.Transforms.SelectColumns(
                    nameof(MLMatch.T1), nameof(MLMatch.T2), nameof(MLMatch.IsPlayedIn3Months), 
                    nameof(MLMatch.IsPlayedIn6Months), "n_rankt1", "n_rankt2", "n_scoret1", "n_scoret2", "n_t1", "n_t2"));

        // var model = trainingPipeline.Fit(dataSet);
        var experimentSettings = new RecommendationExperimentSettings
        {
            CacheDirectoryName = null,
            CancellationToken = ct,
            MaxExperimentTimeInSeconds = 60
        };
        var experiment = mlContext.Auto().CreateRecommendationExperiment(experimentSettings);
        var result = experiment.Execute(dataSet, "label_name", preFeaturizer: trainingPipeline);
        
        
        return false;
        // var learningPipe = trainer.
    }
}