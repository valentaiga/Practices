using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Practices.AzureEventHub.Common;
using Practices.AzureEventHub.Common.Models.Generic;

namespace Practices.AzureEventHub.Producer;

internal static class EventExtensions
{
    public static bool TryAdd<T>(this EventDataBatch batch, EventWrapper<T> dataWrap)
    {
        var serialized = JsonSerializer.SerializeToUtf8Bytes(dataWrap, Global.JsonSerializerOptions);
        var eventData = new EventData(serialized);
        return batch.TryAdd(eventData);
    }
}