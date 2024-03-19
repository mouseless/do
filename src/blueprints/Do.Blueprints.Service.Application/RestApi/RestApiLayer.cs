using Do.Architecture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

using static Do.DependencyInjection.DependencyInjectionLayer;
using static Do.HttpServer.HttpServerLayer;

namespace Do.RestApi;

public class RestApiLayer : LayerBase<AddServices, Build>
{
    readonly IApplicationPartCollection _applicationParts = new ApplicationPartCollection();
    readonly MvcNewtonsoftJsonOptions _mvcNewtonsoftJsonOptions = [];
    readonly SwaggerGenOptions _swaggerGenOptions = new();
    readonly SwaggerOptions _swaggerOptions = new();
    readonly SwaggerUIOptions _swaggerUIOptions = new();

    protected override PhaseContext GetContext(AddServices phase)
    {
        var services = Context.GetServiceCollection();

        services.AddHttpContextAccessor();
        services.AddMvcCore().AddApiExplorer();
        services.AddSwaggerGen();

        return phase.CreateContextBuilder()
            .Add(_applicationParts)
            .Add(_mvcNewtonsoftJsonOptions)
            .Add(_swaggerGenOptions)
            .OnDispose(() =>
            {
                services.AddControllers()
                    .AddNewtonsoftJson(_mvcNewtonsoftJsonOptions)
                    .AddApplicationParts(_applicationParts);
                services.ConfigureSwaggerGen(config =>
                {
                    config.SwaggerGeneratorOptions = _swaggerGenOptions.SwaggerGeneratorOptions;
                    config.SchemaGeneratorOptions = _swaggerGenOptions.SchemaGeneratorOptions;
                    config.ParameterFilterDescriptors = _swaggerGenOptions.ParameterFilterDescriptors;
                    config.RequestBodyFilterDescriptors = _swaggerGenOptions.RequestBodyFilterDescriptors;
                    config.OperationFilterDescriptors = _swaggerGenOptions.OperationFilterDescriptors;
                    config.DocumentFilterDescriptors = _swaggerGenOptions.DocumentFilterDescriptors;
                    config.SchemaFilterDescriptors = _swaggerGenOptions.SchemaFilterDescriptors;
                });
            })
            .Build();
    }

    protected override PhaseContext GetContext(Build phase)
    {
        var app = Context.GetWebApplication();

        app.UseRouting();
        app.MapControllers();

        return phase.CreateContextBuilder()
            .Add(_swaggerOptions)
            .Add(_swaggerUIOptions)
            .OnDispose(() =>
                app
                    .UseSwagger(config =>
                    {
                        config.PreSerializeFilters.AddRange(_swaggerOptions.PreSerializeFilters);
                        config.RouteTemplate = _swaggerOptions.RouteTemplate;
                        config.SerializeAsV2 = _swaggerOptions.SerializeAsV2;
                    })
                    .UseSwaggerUI(config =>
                    {
                        config.ConfigObject = _swaggerUIOptions.ConfigObject;
                        config.DocumentTitle = _swaggerUIOptions.DocumentTitle;
                        config.HeadContent = _swaggerUIOptions.HeadContent;
                        config.IndexStream = _swaggerUIOptions.IndexStream;
                        config.Interceptors = _swaggerUIOptions.Interceptors;
                        config.OAuthConfigObject = _swaggerUIOptions.OAuthConfigObject;
                        config.RoutePrefix = _swaggerUIOptions.RoutePrefix;
                    })
            )
            .Build();
    }
}
