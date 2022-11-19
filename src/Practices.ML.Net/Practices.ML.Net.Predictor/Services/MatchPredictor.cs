using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Trainers.FastTree;
using Practices.ML.Net.Abstractions.Models;
using Practices.ML.Net.Abstractions.Predictor;
using Practices.ML.Net.Predictor.Models;

namespace Practices.ML.Net.Predictor.Services;

internal class MatchPredictor : IMatchPredictor
{
    private readonly IDataTransformer _dataTransformer;
    private readonly ILogger<MatchPredictor> _logger;

    public MatchPredictor(IDataTransformer dataTransformer, ILogger<MatchPredictor> logger)
    {
        _dataTransformer = dataTransformer;
        _logger = logger;
    }

    public MatchPrediction Predict(
        IEnumerable<GameMatch> trainMatches,
        GameMatch matchToPredict,
        CancellationToken ct)
    {
        var matches = trainMatches.Select(_dataTransformer.Transform).ToArray();
        var matchToPred = _dataTransformer.Transform(matchToPredict);
        matchToPred.Team1Win = !matchToPred.Team1Win;
        matchToPred.WinProbabilityT1 = 0f;
        return PredictInternal(matches, matchToPred, ct);
    }

    public MatchPrediction Predict(
        IEnumerable<GameMatch> trainMatches,
        GameMatch matchToPredict)
    {
        var matches = trainMatches.Select(_dataTransformer.Transform).ToArray();
        var matchToPred = _dataTransformer.Transform(matchToPredict);
        matchToPred.Team1Win = !matchToPred.Team1Win;
        matchToPred.WinProbabilityT1 = 0f;
        return PredictInternal(matches, matchToPred);
    }
    
    private MatchPrediction PredictInternal(
        IEnumerable<MLMatch> trainMatches,
        MLMatch matchToPredict,
        CancellationToken ct)
    {
        var labelColumnName = nameof(MLMatch.Team1Win);
        var mlContext = new MLContext(0);
        var trainDataSet = mlContext.Data.LoadFromEnumerable(trainMatches);
        
        var settings = new BinaryExperimentSettings
        {
            CancellationToken = ct,
            CacheDirectoryName = null,
            MaxExperimentTimeInSeconds = 30,
            Trainers =
            {
                BinaryClassificationTrainer.FastForest // works better than anything else in this case
            }
        };

        var transformer = BuildTrainingPipeline(mlContext);

        var experiment = mlContext.Auto().CreateBinaryClassificationExperiment(settings);
        _logger.LogDebug("Data training start ({count} matches to train on total)", trainMatches.Count());
        
        var trainRun = experiment.Execute(
            trainData: trainDataSet,
            labelColumnName: labelColumnName,
            preFeaturizer: transformer);

        var predictionEngine = mlContext.Model.CreatePredictionEngine<MLMatch, MLPrediction>(trainRun.BestRun.Model);
        
        var prediction = predictionEngine.Predict(matchToPredict);
        
        _logger.LogInformation("Prediction: Team 1 win - {t1win}, score - {score}",
            prediction.Team1Win,
            prediction.Score);

        return new MatchPrediction(prediction.Team1Win, prediction.Score);
    }

    private MatchPrediction PredictInternal(
        IEnumerable<MLMatch> trainData,
        MLMatch match)
    {
        var labelColumnName = nameof(MLMatch.WinProbabilityT1);
        var mlContext = new MLContext(0);
        _logger.LogInformation("Data load started");
        var trainDataSet = mlContext.Data.LoadFromEnumerable(trainData);
        var predictDataSet = mlContext.Data.LoadFromEnumerable(new[] { match });

        _logger.LogInformation("Pipeline fit & transform started");
        var normalizedDataSet = BuildTestPipeline(mlContext, labelColumnName).Fit(trainDataSet).Transform(predictDataSet);
        var options = new FastTreeRankingTrainer.Options
        {
            RowGroupColumnName = labelColumnName,
            FeatureFirstUsePenalty = 0.1,
            NumberOfTrees = 50,
            NumberOfLeaves = 1024,
            MinimumExampleCountPerLeaf = 20,
            // FeatureColumnName = nameof(MLMatch.VectorA),
            LabelColumnName = nameof(MLMatch.T1WinMaps),

            // LearningRate = 1D,
            // FeatureColumnName = labelColumnName,
            // FeatureFraction = 0.9
        };
        _logger.LogInformation("Model train started");
        var trainer = mlContext.Ranking.Trainers.FastTree(options);
        _logger.LogInformation("Test data transform started");
        var model = trainer.Fit(normalizedDataSet);
        var transformed = model.Transform(normalizedDataSet);

        _logger.LogInformation("Prediction is ready");
        var prediction = mlContext.Data.CreateEnumerable<MLMapPrediction>(transformed, false).First();
        _logger.LogInformation("Prediction: Team 1 win - {t1win} maps of 3, score - {score}",
            prediction.MapsWin,
            prediction.Score);
        
        return new MatchPrediction(prediction.Probability > 0.5, prediction.Score);
    }

    private static IEstimator<ITransformer> BuildTestPipeline(MLContext mlContext, string labelColumn)
        => mlContext.Transforms.NormalizeMeanVariance("n_probt1", nameof(MLMatch.WinProbabilityT1))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t1", nameof(MLMatch.T1P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t2", nameof(MLMatch.T2P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t3", nameof(MLMatch.T3P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t4", nameof(MLMatch.T4P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t5", nameof(MLMatch.T5P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t6", nameof(MLMatch.T6P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t7", nameof(MLMatch.T7P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t8", nameof(MLMatch.T8P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t9", nameof(MLMatch.T9P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t10", nameof(MLMatch.T10P)))
            .Append(mlContext.Transforms.CopyColumns("Features", nameof(MLMatch.VectorA)))
            .Append(mlContext.Transforms.CopyColumns("Example", "n_probt1"))
            // .Append(mlContext.Transforms.Concatenate("Label", new []{"n_t1","n_t2","n_t3","n_t4","n_t5","n_t6","n_t7","n_t8","n_t9","n_t9","n_t10",}))
        ;

    private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        => mlContext.Transforms.NormalizeMeanVariance("n_probt1", nameof(MLMatch.WinProbabilityT1))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t1", nameof(MLMatch.T1P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t2", nameof(MLMatch.T2P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t3", nameof(MLMatch.T3P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t4", nameof(MLMatch.T4P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t5", nameof(MLMatch.T5P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t6", nameof(MLMatch.T6P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t7", nameof(MLMatch.T7P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t8", nameof(MLMatch.T8P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t9", nameof(MLMatch.T9P)))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding("n_t10", nameof(MLMatch.T10P)))
            // .Append(mlContext.Transforms.DropColumns(
            //     nameof(MLMatch.T1P), nameof(MLMatch.T2P), nameof(MLMatch.T3P), nameof(MLMatch.T4P), nameof(MLMatch.T5P), 
            //     nameof(MLMatch.T6P), nameof(MLMatch.T7P), nameof(MLMatch.T8P), nameof(MLMatch.T9P), nameof(MLMatch.T10P)))
        ;
}