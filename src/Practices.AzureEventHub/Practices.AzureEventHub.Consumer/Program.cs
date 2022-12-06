using Practices.AzureEventHub.Common.Options;
using Practices.AzureEventHub.Consumer;

await Host.CreateDefaultBuilder()
    .ConfigureDefaults(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<ConsumerHubOptions>(
            context.Configuration.GetSection("AzureEventHub"));
        
        // consumers save checkpoints in blob storage, so its required for consistency (otherwise event double possible)
        services.Configure<BlobStorageOptions>(
            context.Configuration.GetSection("AzureBlobStorage"));
        
        // main action goes here
        services.AddHostedService<EventHubProcessor>();
    })
    .RunConsoleAsync();