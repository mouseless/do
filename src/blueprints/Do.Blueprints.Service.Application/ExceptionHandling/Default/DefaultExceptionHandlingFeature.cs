using Do.Architecture;
using Do.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Do.ExceptionHandling.Default;

public class DefaultExceptionHandlingFeature(Setting<string>? _typeUrlFormat = default)
    : IFeature<ExceptionHandlingConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureDomainTypeCollection(types =>
        {
            types.Add<HandledException>();
            types.Add<IExceptionHandler>();
        });

        configurator.ConfigureServiceCollection(services =>
        {
            services.AddSingleton<IExceptionHandler, UnauthorizedAccessExceptionHandler>();
            services.AddSingleton<IExceptionHandler, HandledExceptionHandler>();
            services.AddSingleton(new ExceptionHandlerSettings(_typeUrlFormat));

            services.AddExceptionHandler<ExceptionHandler>();
            services.AddProblemDetails();
        });

        configurator.ConfigureMiddlewareCollection(middlewares =>
        {
            middlewares.Add(app =>
                {
                    app.UseExceptionHandler();
                    app.UseStatusCodePages();
                },
                order: -10
            );
        });
    }
}
