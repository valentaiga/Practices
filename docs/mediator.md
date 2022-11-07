# Mediator
A high performance implementation of Mediator pattern in .NET using source generators.

[**Original GitHub project**](https://github.com/martinothamar/Mediator)
## Why and when?
The mediator design pattern is useful when the number of objects grows so large that it becomes difficult to maintain the references to the objects.  
The mediator is essentially an object that encapsulates how one or more objects interact with each other.

Useful for:
- High performance
  - Runtime performance can be the same for both runtime reflection and source generator based approaches, but it's easier to optimize in the latter case
- Build time errors instead of runtime errors
- AOT friendly _(not that useful for me)_
    - MS are investing time in various AOT scenarios, and for example iOS requirees AOT compilation

## How to use
DI - scans for all Handlers and registers them:
```csharp
services.AddMediator(options => 
{
    // setting
});
```
Service use:
```csharp
// use
ctor(IMediator mediator)

var service = _mediatr.Send(MethodRequest.Instance);
```
```csharp
// request
public record MethodRequest : IRequest<Response>
{
    public static readonly MethodRequest = new();
    
    private MethodRequest(){}
}
```
```csharp
// tipically its your service realization
public class MethodHandler : 
    IRequestHandler<Request, Response>
{
    public ValueTask<Response> Handle(
        Request request, CancellationToken ct)
    {
        // ... some logic
    }
}
```
