# Reactive Extensions .NET (Rx)
The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators.

## Interlude
Using Rx, developers **represent** asynchronous data streams with Observables, **query** asynchronous data streams using LINQ operators, and **parameterize** the concurrency in the asynchronous data streams using Schedulers.  
Simply put, Rx = Observables + LINQ + Schedulers.

### IEnumerable\<T\>
We start from enumerable.
- Core interface for objects like List<T>  
- Provides as way to iterate through items  
- Generally comes as _prepackaged data_ that we open up and comb through
- We PULL data from it

### IObservable\<T\>
At this point we're not just listening for individual changes or single big change.
Now we set a conveyor belt (or a stream) of changes coming over time.

### Subscribing
- Subscriber listens for observable and gets values from it. 
- Sub can have multiple subscriptions.
- Unsubscribe using IDisposable (sub disposes when stops to listen)
- Linq compatible

## When and Why?
- Multiple data producers (UI with real time validation)
- Server side processing (to make async even better)

### What Rx provides?
- Buffer (periodically gather items and emit them in batch)
- Throttle (only get a value after period of time has passed)
- Merge multiple observables into a single stream
- Distinct values (only allow unique values from an observable)
- Interval (emit a notification after a period of time has passed)
- Etc... (a list is not over)