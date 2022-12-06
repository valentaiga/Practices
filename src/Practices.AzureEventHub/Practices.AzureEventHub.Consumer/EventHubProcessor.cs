using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Practices.AzureEventHub.Common;
using Practices.AzureEventHub.Common.Models;
using Practices.AzureEventHub.Common.Models.Generic;
using Practices.AzureEventHub.Common.Options;

namespace Practices.AzureEventHub.Consumer;

public class EventHubProcessor : BackgroundService
{
    private readonly ILogger<EventHubProcessor> _logger;
    private readonly ConsumerHubOptions _hubOptions;
    private readonly BlobStorageOptions _storageOptions;

    public EventHubProcessor(
        ILogger<EventHubProcessor> logger,
        IOptions<ConsumerHubOptions> hubOptions,
        IOptions<BlobStorageOptions> storageOptions)
    {
        _logger = logger;
        _hubOptions = hubOptions.Value;
        _storageOptions = storageOptions.Value;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var storageClient = new BlobContainerClient(
            _storageOptions.ConnectionString,
            _storageOptions.ContainerName);

        var processor = new EventProcessorClient(
            storageClient,
            _hubOptions.ConsumerGroup,
            _hubOptions.ConnectionString,
            _hubOptions.HubName);
        
        processor.ProcessEventAsync += ProcessEventHandler;
        processor.ProcessErrorAsync += ProcessErrorHandler;

        await processor.StartProcessingAsync(stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_hubOptions.ListenTimeout, stoppingToken);
        }
        await processor.StopProcessingAsync(stoppingToken);
    }

    private async Task ProcessEventHandler(ProcessEventArgs arg)
    {
        var data = await JsonSerializer.DeserializeAsync<EventWrapper<SpecificData>>(
            arg.Data.BodyAsStream,
            Global.JsonSerializerOptions);

        if (data?.Data is null)
        {
            var text = Encoding.UTF8.GetString(arg.Data.Body.Span);
            _logger.LogError("Unexpected event received. Cant deserialize to {type}. Text: '{text}'",
                nameof(EventWrapper<SpecificData>),
                text);
            return;
        }
        
        _logger.LogInformation(
            "Received event with id {id} and text '{text}' at {timestamp:u}. Partition id {partitionId}", 
            data.Data.Id, 
            data.Data.Text,
            data.Timestamp,
            arg.Partition.PartitionId);
        await arg.UpdateCheckpointAsync();
    }

    private Task ProcessErrorHandler(ProcessErrorEventArgs arg)
    {
        _logger.LogInformation("Exception in {partition}. Message: {message}. Stacktrace: {trace}",
            arg.PartitionId,
            arg.Exception.Message,
            arg.Exception.StackTrace);
        return Task.CompletedTask;
    }
}