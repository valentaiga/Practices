using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using Practices.AzureEventHub.Common.Models;
using Practices.AzureEventHub.Common.Models.Generic;
using Practices.AzureEventHub.Common.Options;

namespace Practices.AzureEventHub.Producer;

/// <summary>
/// Sends 3 events every 30 seconds in hub
/// </summary>
internal class EventHubSender : BackgroundService
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);
    private int _eventsCreated = 0;
    
    private readonly EventHubOptions _options;
    private readonly ILogger<EventHubSender> _logger;

    public EventHubSender(
        IOptions<EventHubOptions> options,
        ILogger<EventHubSender> logger)
    {
        _logger = logger;
        _options = options.Value;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var producerClient = new EventHubProducerClient(
            _options.ConnectionString, _options.HubName);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var eventBatch = await producerClient.CreateBatchAsync(stoppingToken);
            
            for (var i = 0; i < 3; i++)
            {
                var data = GenerateObject(_eventsCreated++);
                _logger.LogInformation("Object with id {id} generated",
                    data.Data!.Id);
                eventBatch.TryAdd(data);
            }
            
            await producerClient.SendAsync(eventBatch, stoppingToken);
            _logger.LogInformation("Batch with {count} events sent",
                eventBatch.Count);
            await Task.Delay(_timeout, stoppingToken);
        }
    }
    
    private static EventWrapper<SpecificData> GenerateObject(int id)
    {
        var timestamp = DateTime.UtcNow;
        var data = new SpecificData(id, $"New event #{id}");
        return new EventWrapper<SpecificData>(data, timestamp);
    }
}