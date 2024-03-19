# Rest API

DO uses ASP.NET Core for configuring and building a REST API application.

```csharp
app.Layers.AddRestApi();
```

## Configuration Targets

This layer provides `IApplicationPartCollection` for registering necessary
application parts, `MvcNewtonsoftJsonOptions` `SwaggerGenOptions`, 
`SwaggerOptions` and `SwaggerUIOptions` for configuring `Swagger` behavior.

### `IApplicationPartCollection`

This target is provided in `AddServices` phase. To configure it in a feature;

```csharp
configurator.ConfigureApplicationParts(applicationParts =>
{
    ...
});
```

### `MvcNewtonsoftJsonOptions`

This target is provided in `AddServices` phase. To configure it in a feature;

```csharp
 configurator.ConfigureMvcNewtonsoftJsonOptions(options =>
 {
     ...
 });
```

### `SwaggerGenOptions`

This target is provided in `AddServices` phase right after
`IApplicationPartCollection`. To configure it in a feature;

```csharp
configurator.ConfigureSwaggerGenOptions(swaggerGenOptions =>
{
    ...
});
```

### `SwaggerOptions`

This target is provided in `AddServices` phase right after
`SwaggerGenOptions`. To configure it in a feature;

```csharp
configurator.ConfigureSwaggerOptions(swaggerOptions =>
{
    ...
});
```

### `SwaggerUIOptions`

This target is provided in `AddServices` phase right after
`SwaggerOptions`. To configure it in a feature;

```csharp
configurator.ConfigureSwaggerUIOptions(swaggerUIOptions =>
{
    ...
});
```
