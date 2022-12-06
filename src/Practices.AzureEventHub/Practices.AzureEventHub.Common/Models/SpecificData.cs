using System.Text.Json.Serialization;

namespace Practices.AzureEventHub.Common.Models;

public class SpecificData
{
    public int Id { get; set; }
    public string? Text { get; set; }

    public SpecificData(int id, string text)
    {
        Id = id;
        Text = text;
    }

    [JsonConstructor]
    private SpecificData()
    {
    }
}