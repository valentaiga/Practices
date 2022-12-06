using Practices.AzureEventHub.Common.Options;
using Practices.AzureEventHub.Producer;

await Host.CreateDefaultBuilder()
    .ConfigureDefaults(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<EventHubOptions>(
            context.Configuration.GetSection("AzureEventHub"));
        
        // main action goes here
        services.AddHostedService<EventHubSender>();
    })
    .RunConsoleAsync();