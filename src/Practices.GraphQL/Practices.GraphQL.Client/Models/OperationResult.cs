namespace Practices.GraphQL.Client.Models;

public class OperationResult<TResult> : OperationResult
{
    public TResult Result { get; }

    public OperationResult(TResult result)
    {
        Result = result;
    }

    public OperationResult(GraphQLException error) : base(error)
    {
    }
}

public class OperationResult
{
    public GraphQLException Error { get; }
    public bool IsError => Error is null;

    internal OperationResult()
    {
    }

    protected OperationResult(GraphQLException error)
    {
        Error = error;
    }
    
    public static OperationResult<TResult> Success<TResult>(TResult result)
        => new OperationResult<TResult>(result);

    public static OperationResult<TResult> Fail<TResult>(GraphQLException exception)
        => new OperationResult<TResult>(exception);
}