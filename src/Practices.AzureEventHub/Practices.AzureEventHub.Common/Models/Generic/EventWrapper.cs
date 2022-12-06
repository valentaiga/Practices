using System.Text.Json.Serialization;

namespace Practices.AzureEventHub.Common.Models.Generic;

public class EventWrapper<TValue>
{
    public DateTime Timestamp { get; init; }
    public TValue? Data { get; init; }

    public EventWrapper(TValue? data, DateTime timestamp) =>
        (Data, Timestamp) = (data, timestamp);

    [JsonConstructor]
    private EventWrapper()
    {
    }
}