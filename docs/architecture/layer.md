# Layer

Each `ILayer` implementation represents an internal system component of the
software you develop. A layer provides application features with configuration
of this system component so that the features can bind domain objects and
system components together.

Let's take relational database as an example. A relational database is treated
as an internal system component and is introduced by the `DataAccessLayer` along
with a configuration API. This API enables a feature to map your domain entities
to database tables.

Another good example is the `HttpServerLayer`. This layer introduces the ASP.NET
Core framework into your application. It provides three phases, as mentioned in
[Application](./application.md), along with `IMiddlewareCollection` and
`IEndpointRouteBuilder` objects as its configuration API. This way, any feature
has the ability to use a middleware or add routes to the application.

## Conventions

For a consistent developer experience, follow below conventions when
implementing a new layer;

1. Place all layer related classes under the same folder named after layer,
   e.g., `DependencyInjection/`
1. Use `Layer` suffix in layer class name, e.g.,
   `DependencyInjection/DependencyInjectionLayer.cs`
1. Provide extension methods in `Do` namespace, e.g.,
   `DependencyInjection/DependencyInjectionExtensions.cs`;
   1. `Add` extension to `List<ILayer>`, e.g., `AddDependencyInjection()`
   1. `Get` extensions to `ApplicationContext`, e.g.,
      `GetWebApplicationBuilder()`, `GetWebApplication()`
   1. `Configure` extensions to `LayerConfigurator` per configuration
      target(s), e.g., `ConfigureServiceCollection()`
1. Place phase implementations as nested classes under the layer class
1. Don't use any suffix for phases and use method-like names, e.g., `Build` and
   `Run`

Please refer to existing layers in [github.com/mouseless/do][] for examples.

## Creating A Layer

To create a layer, create a class using above conventions and extend
`LayerBase`.

```csharp
public class LayerX : LayerBase
{
    ...
}
```

> :bulb:
>
> You can directly implement provided interface `ILayer`, however we've created
> some base classes to make it easier for you to create layers. All examples
> demonstrate usage of these base classes.

### `Id` of a Layer

`ILayer` has an `Id` property which should be a unique per `Application`
instance. `LayerBase` sets the `Id` as name of the concrete class e.g.,
`LayerX` at above example.

You can override its value in layer implementation class as shown below;

```csharp
public class LayerX : LayerBase
{
    protected override string Id => "CustomUniqueId"
    ...
}
```

## Adding Phases

By default `LayerBase` returns no phases. To add one or more phases into the
application, you need to override `GetPhases()` method as shown below;

```csharp
public class SampleLayer : LayerBase
{
    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new DoA();
        yield return new DoB();
    }

    public class DoA : PhaseBase { ... }
    public class DoB : PhaseBase { ... }
}
```

Here `SampleLayer` adds two phases, `DoA` and `DoB`, to the application.

> :bulb:
>
> By convention place the phase implementations as a nested class in the layer
> it is added.

### Initializing a Phase

`PhaseBase` class has `Initialize()` method to override so that it can add an
object to the application context via `Context` property. Base `Initialize()`
method does not add any objects to the application. Below, you can see how
`CreateBuilder` phase adds `WebApplicationBuilder` to the context;

```csharp
public class CreateBuilder : PhaseBase
{
    protected override void Initialize()
    {
        var build = WebApplication.CreateBuilder();

        Context.Add(build);
    }
}
```

> :bulb:
>
> You can directly implement provided interface `IPhase`, however we've created
> some base classes to make it easier for you to create phases. All examples
> demonstrate usage of these base classes.

### Readiness via Dependencies

You can define context dependencies via generic `PhaseBase<>` classes to make
the phase wait until context is ready to provide that dependency. For example,
`Build` depends on the `WebApplicationBuilder` instance as shown below;

```csharp
public class Build : PhaseBase<WebApplicationBuilder>
{
    protected override void Initialize(WebApplicationBuilder build)
    {
        var app = build.Build();

        Context.Add(app);
    }
}
```

`PhaseBase<T>` class requires `Initialize(T t)` method to be implemented.

> :information_source:
>
> You can provide more than one dependency for a phase. E.g., `Phase<X, Y>`
> will require `Initialize(X x, Y y)` method to be implemented.

> :warning:
>
> Type of the dependency provided, must be exactly the same as the type of
> dependency in `ApplicationContext`. For more information
> [Running an Application](../architecture/application.md#running-an-application)

Phases can also define context dependencies from other phase artifacts. For 
example `HttpServerLayer.Build` phase can require `IServiceCollection` which 
will be provided during `DependencyInjectionLayer.AddServices` phase. 

```csharp
// DependencyInjectionLayer
public class AddServices(IServiceCollection _services)
    : PhaseBase(PhaseOrder.Early)
{
    protected override void Initialize()
    {
        Context.Add(_services);
    }
}

// HttpServerLayer
public class Build()
    : PhaseBase<WebApplicationBuilder, IServiceCollection>(PhaseOrder.Latest)
{
    protected override void Initialize(WebApplicationBuilder build, IServiceCollection services)
    {
        ...
    }
}
```

> :warning:
>
> This type of artifact requirement will create a dependency between phases 
> which results to a layer to layer dependency. In the above example `Build`
> phase will never execute unless `DependencyLayer` is added to the application

### Order of a Phase

`PhaseBase` classes has `order` parameter in their constructors. Default order
is `PhaseOrder.Normal`. If you need to change the order, pass the desired order
to this parameter as shown below;

```csharp
public class DoThisEarlyOn()
    : PhaseBase(PhaseOrder.Early)
{ }
```

## Providing Configuration

Layers provide configuration objects phase by phase. This means a layer can
provide two different configuration objects for two different phases. For this
reason, a layer returns `PhaseContext` instance per phase to which it provides
configuration.

```csharp
public class LayerX : LayerBase<AddServices>
{
    readonly LayerXConfiguration _configuration = new();

    protected override PhaseContext GetContext(AddServices phase) =>
        phase.CreateContext(_configuration);
}
```

In this example, you see a layer named `LayerX` providing a
`LayerXConfiguration` instance during `AddServices` phase.

> :information_source:
>
> By default, a layer returns `PhaseContext.Empty` instance for the phases it
> does not provide a configuration. This means `Application` skips that layer
> for the phases it doesn't have anything to configure.

### Using non-generic `LayerBase`

`LayerBase<>` classes allow up to three generic arguments. If you need to
implement a layer that has things to configure during more than three phases,
use non-generic `LayerBase` class, override `GetContext(IPhase phase)` method
and switch given phase according to its type;

```csharp
public class MyLayer : LayerBase
{
    protected override PhaseContext GetContext(IPhase phase) =>
        phase switch
        {
            PhaseA => phase.CreateContext(...),
            PhaseB => phase.CreateContext(...),
            _ => base.GetContext(phase)
        };
}
```

### Before and After a Phase

Layers might need to do some standard stuff before and after a phase is applied
to all features for all layers. Use `GetContext()` method to do stuff before,
and `onDispose:` delegate to do stuff after.

```csharp
public class LayerX : LayerBase<AddServices>
{
    readonly LayerXConfiguration _configuration = new();

    protected override PhaseContext GetContext(AddServices phase)
    {
        var services = Context.Get<IServiceCollection>();

        services.AddStandardStuffBefore();

        phase.CreateContext(_configuration,
            onDispose: () => services.AddStandardStuffAfter()
        );
    }
}
```

> :information_source:
>
> Notice that a layer has access to the `ApplicationContext` instance through
> `Context` property provided in `LayerBase` base class.

> :warning:
>
> `GetContext()` method is called for every layer before applying any of them
> to features. And `onDispose:` actions are called after applying these
> contexts to all features.

### Providing Multiple Targets

It is possible to provide up to three configuration objects for the same phase.
There are two ways of providing multiple targets; at once or one by one.

#### At Once

Below code demonstrates providing two configuration objects at once;

```csharp
public class LayerX : LayerBase<AddServices>
{
    readonly Configuration1 _configuration1 = new();
    readonly Configuration2 _configuration2 = new();

    protected override PhaseContext GetContext(AddServices phase) =>
        phase.CreateContext(_configuration1, _configuration2);
}
```

This phase context will require an action with two parameter in a feature;

```csharp
...
public void Configure(LayerConfigurator configurator)
{
    configurator.Configure((Configuration1 configuration1, Configuration2 configuration2) =>
    {
        // use both objects to configure
    });
}
...
```

See [Feature](./feature.md) for more information on using layer configurators
for features.

#### One by One

Below code demonstrates providing configuration objects one by one;

```csharp
public class LayerX : LayerBase<AddServices>
{
    readonly Configuration1 _configuration1 = new();
    readonly Configuration2 _configuration2 = new();

    protected override PhaseContext GetContext(AddServices phase) =>
        phase.CreateContextBuilder()
            .Add(_configuration1)
            .Add(_configuration2)
            .Build()
        ;
}
```

This phase context will require two different actions in a feature;

```csharp
...
public void Configure(LayerConfigurator configurator)
{
    configurator.Configure((Configuration1 configuration1) =>
    {
        // configure one by one
    });

    configurator.Configure((Configuration2 configuration2) =>
    {
        // configure one by one
    });
}
...
```

> :bulb:
>
> `phase.CreateContext()` is a helper method that utilizes
> `phase.CreateContextBuilder()` behind the scenes.

#### Using in Combination

You may combine these two ways to provide configuration;

```csharp
public class LayerX : LayerBase<AddServices>
{
    readonly Configuration1 _configuration1 = new();
    readonly Configuration2 _configuration2 = new();
    readonly Configuration3 _configuration3 = new();

    protected override PhaseContext GetContext(AddServices phase) =>
        phase.CreateContextBuilder()
            .Add(_configuration1, _configuration2)
            .Add(_configuration3)
            .Build()
        ;
}
```

This phase context will require two different actions;

1. For the first two configuration objects: `(Configuration1 configuration1,
   Configuration2 configuration2)`
1. For the last configuration object: `(Configuration3 configuration3)`.

[github.com/mouseless/do]:https://github.com/mouseless/do
