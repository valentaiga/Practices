using System.Text.Json.Serialization;
using GraphQL;
using GraphQL.SystemTextJson;

namespace Practices.GraphQL.GraphQL;

public class GraphQLQuery
{
    public string? OperationName { get; set; }
    public string? Query { get; set; }
    [JsonConverter(typeof(InputsJsonConverter))] public Inputs? Variables { get; set; }
}