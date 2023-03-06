using System.Collections.Immutable;
using FilesProcessor.Options;
using Microsoft.Extensions.Options;

namespace FilesProcessor.Services;

public class BatchCompressor : IBatchCompressor
{
    private readonly ILogger<BatchCompressor> _logger;
    private readonly IFileReader _fileReader;

    public BatchCompressor(
        ILogger<BatchCompressor> logger,
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }

    public async Task<ImmutableArray<string>> FormBatch()
    {
        var batch = new List<string>(100);
        await foreach (var jsonLine in _fileReader.ReadNextFileLinesAsync())
        {
            batch.Add(jsonLine);
        }

        return batch.ToImmutableArray();
    }
}

public interface IBatchCompressor
{
    Task<ImmutableArray<string>> FormBatch();
}