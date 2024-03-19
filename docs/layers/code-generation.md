# Code Generation

Allows meta-programming-like experience to reduce repetitive code.

```csharp
app.Layers.AddCodeGeneration();
```

## Configuration Targets

Code generation layer provides `IGeneratedAssemblyCollection` as the only
configuration target.

### `IGeneratedAssemblyCollection`

This target is provided in `GenerateCode` phase. To configure it in a feature;

```csharp
configurator.ConfigureGeneratedAssemblyCollection(assemblies =>
{
    ...
});
```

## Phases

This layer introduces following phases to the application it is added;

- `GenerateCode`: This phase creates a `IGeneratedAssemblyCollection` instance
  and places it in the application context
- `Compile`: This phase compiles generated code during above phase, and places
  all generated assemblies into `GeneratedAssemblyProvider`, which is added to
  the application context

> :bulb:
>
> To access to a generated assembly from a feature use
> `configurator.Context.GetGeneratedAssembly("MyAssembly")` extension method.
