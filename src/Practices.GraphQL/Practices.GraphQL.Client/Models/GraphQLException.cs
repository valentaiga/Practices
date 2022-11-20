using System.Text;
using GraphQL;

namespace Practices.GraphQL.Client.Models;

public sealed class GraphQLException : Exception
{
    public string Error { get; }

    public GraphQLException(string error)
    {
        Error = error;
    }

    public static GraphQLException FromException(Exception exception)
        => new GraphQLException(exception.ToString());
    
    public static GraphQLException FromResponse(IGraphQLResponse response)
    {
        if (response.Errors is null)
            throw new ArgumentNullException(
                nameof(response.Errors), 
                "Can not build GraphQL exception from success response.");
                
        var sb = new StringBuilder();
        sb.Append("Unknown GraphQL error. ");
        foreach (var error in response.Errors)
        {
            sb.AppendFormat("Message: '{0}'. {1}", error.Message, Environment.NewLine);

            if (error.Locations is null)
                continue;
            
            foreach (var location in error.Locations)
            {
                sb.AppendFormat("\tLocation: {0} line, {1} column.{2}", 
                    location.Line, 
                    location.Column, 
                    Environment.NewLine);
            }
        }

        return new GraphQLException(sb.ToString());
    }
}