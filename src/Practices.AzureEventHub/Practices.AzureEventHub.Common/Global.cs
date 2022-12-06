using System.Text.Json;

namespace Practices.AzureEventHub.Common;

public static class Global
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = 
        new JsonSerializerOptions { };
}