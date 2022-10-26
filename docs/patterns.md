# Patterns
_Design patterns are solutions to software design problems you find again and again in real-world application development. Patterns are about reusable designs and interactions of objects._

## Creational Patterns

### Abstract Factory
Creates an instance of several families of classes
### Factory Method
Creates an instance of several derived classes
### Singleton
A class of which only a single instance can exist
### Builder	
Separates object construction from its representation
### Prototype
A fully initialized instance to be copied or cloned

## Structural Patterns

### Facade
The Facade design pattern provides a unified interface to a set of interfaces in a subsystem.  
This pattern defines a higher-level interface that makes the subsystem easier to use.
### Proxy
The Proxy design pattern provides a surrogate or placeholder for another object to control access to it.
### Adapter
The Adapter design pattern converts the interface of a class into another interface clients expect.  
This design pattern lets classes work together that couldn‘t otherwise because of incompatible interfaces.
### Composite
The Composite design pattern composes objects into tree structures to represent part-whole hierarchies.  
This pattern lets clients treat individual objects and compositions of objects uniformly.  
_(helps to build a tree)_
### Bridge
Separates an object’s interface from its implementation
### Decorator
Add responsibilities to objects dynamically
### Flyweight
A fine-grained instance used for efficient sharing

## Behavioral Patterns

### Iterator
Sequentially access the elements of a collection
### Observer
The Observer design pattern defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
### Strategy
Encapsulates an algorithm inside a class
### Command
The Command design pattern encapsulates a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.
### Chain of Resp.
A way of passing a request between a chain of objects
### State
The State design pattern allows an object to alter its behavior when its internal state changes.  
The object will appear to change its class.
### Mediator
The Mediator design pattern defines an object that encapsulates how a set of objects interact.  
Mediator promotes loose coupling by keeping objects from referring to each other explicitly, and it lets you vary their interaction independently.
### Memento
Capture and restore an object's internal state
### Template Method
Defer the exact steps of an algorithm to a subclass
### Visitor
Defines a new operation to a class without change
### Interpreter
A way to include language elements in a program

# Solid
The **SOLID** principles are five principles of Object-Oriented class design.  
They are a set of rules and best practices to follow while designing a class structure.  
These five principles help to understand the need for certain design patterns and software architecture in general.

## Single Responsibility Principle
_Class should do one thing and therefore it should have only a single reason to change._

**Example**: if a class is a data container, like a Book class or a Student class, and it has some fields regarding that entity, it should change only when we change the data model.

Why:
- easier to work for many different teams
- makes version control easier (data change, merge conflicts)

## Open-Closed Principle
Classes should be open for extension and closed to modification.  
Usually interfaces help us with that principle.  

Why:
- risk of bugs creation
- tested/reliable code should be untouched

## Liskov Substitution Principle
_Subclasses should be substitutable for their base classes._  
When we use inheritance we assume that the child class inherits everything that the superclass has. The child class extends the behavior but never narrows it down.    

Example is the `Rectangle` with `getArea()` func, which can be overrided by `square`, but `sideA` and `sideB` cannot be overrided. 

## Interface Segregation Principle
The Interface Segregation Principle is about separating the interfaces.
_Many client-specific interfaces are better than one general-purpose interface. Clients should not be forced to implement a function they do no need._

## Dependency Inversion Principle
_Classes should depend upon interfaces or abstract classes instead of concrete classes and functions._

# Useful links
- (Patterns list)[https://www.dofactory.com/net/mediator-design-pattern]