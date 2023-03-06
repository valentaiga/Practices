using FilesProcessor.Options;
using Microsoft.Extensions.Options;

namespace FilesProcessor.Services;

public class FileReader : IFileReader
{
    private readonly ILogger<FileReader> _logger;
    private readonly ListenerSettings _settings;
    private readonly SemaphoreSlim _locker = new(1, 1);
    private readonly HashSet<string> _processingFiles = new();

    public FileReader(
        ILogger<FileReader> logger,
        IOptions<ListenerSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;

        if (string.IsNullOrWhiteSpace(_settings.Directory))
            throw new ApplicationException(
                "Files Directory setting is not defined. Please define it in appsettings.json");

        if (!Path.Exists(_settings.Directory))
            throw new ApplicationException(
                "Files Directory does not exist. Please define it in appsettings.json");
    }

    // public bool NewFileAvailable()
    // {
    //     foreach (var filePath in Directory.EnumerateFiles(_settings.Directory!))
    //     {
    //         var filename = Path.GetFileName(filePath);
    //
    //         if (!_processingFiles.Contains(filename))
    //             return true;
    //     }
    //
    //     return false;
    // }
    
    public async IAsyncEnumerable<string> ReadNextFileLinesAsync()
    {
        foreach (var filePath in Directory.EnumerateFiles(_settings.Directory!))
        {
            var filename = Path.GetFileName(filePath);
            
            await _locker.WaitAsync();
            if (_processingFiles.Contains(filename))
            {
                _locker.Release();
                continue;
            }
            _processingFiles.Add(filename);
            _locker.Release();
            
            _logger.LogInformation("File '{0}' reading started", filename);

            await foreach (var line in File.ReadLinesAsync(filePath))
            {
                yield return line;
            }
        
            _logger.LogInformation("File '{0}' reading finished", filename);
            File.Delete(filePath);
            _processingFiles.Remove(filename);

            yield break; 
        }
    }
}

public interface IFileReader
{
    // bool NewFileAvailable();
    IAsyncEnumerable<string> ReadNextFileLinesAsync();
}