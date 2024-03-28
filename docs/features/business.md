# Business

Implementations of this feature will be customized for your own project needs.
A built-in default implementation is provided which configures and uses the 
generated a `DomainModel` instance to register components to 
`IServiceCollection`.

Add this feature implementations using `AddBusiness()` extension;

```csharp
app.Features.AddBusiness(...);
```

## Default

Adds default opinionated business feature with given assemblies and controller 
assembly.

```csharp
c => c.Default(assemblies: [typeof(MyEntity).Assembly])
```
