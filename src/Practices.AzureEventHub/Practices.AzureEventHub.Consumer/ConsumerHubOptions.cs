using Practices.AzureEventHub.Common.Options;

namespace Practices.AzureEventHub.Consumer;

public class ConsumerHubOptions : EventHubOptions
{
    public string? ConsumerGroup { get; set; }
    public TimeSpan ListenTimeout { get; set; }
}