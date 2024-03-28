# Domain

DO introduces a model generation mechanism to reflect the business domain of
a project. The generated model instance can be used in directly in layers or 
in features while configuring configuration targets.

```csharp
app.Layers.AddDomain();
```

## Configuration Targets

This layer provides `IDomainAssemblyCollection`, `IDomainTypeCollection` and 
`DomainBuilderOptions`, `DomainConventionCollection`, `DomainIndexers` as 
configuration targets for building `DomainModel`.

### `IDomainAssemblyCollection`

This target is provided in `BuildConfiguration` phase. To configure it in a 
feature;

```csharp
configurator.ConfigureDomainAssemblyCollection(assemblies =>
{
    ...
});
```

### `IDomainTypeCollection`

This target is provided in `BuildConfiguration` phase. To configure it in a 
feature;

```csharp
configurator.ConfigureDomainTypeCollection(types =>
{
    ...
});
```

### `DomainBuilderOptions`

This target is provided in `BuildConfiguration` phase. To configure it in a 
feature;

```csharp
configurator.ConfigureDomainBuilderOptions(options =>
{
    ...
});
```

### `DomainConventionCollection`

This target is provided in `BuildConfiguration` phase. To configure it in a 
feature;

```csharp
configurator.ConfigureDomainMetaData(metadata =>
{
    ...
});
```

### `DomainIndexerCollection`

This target is provided in `BuildConfiguration` phase. To configure it in a 
feature;

```csharp
configurator.ConfigureDomainIndexers(indexers =>
{
    ...
});
```

## Phases

This layer introduces following phases to the application it is added;

- `BuildDomain`: This phase adds an `DomainModel` instance to the application
  context and runs before `AddServices` to allow the access to `DomainModel`.
