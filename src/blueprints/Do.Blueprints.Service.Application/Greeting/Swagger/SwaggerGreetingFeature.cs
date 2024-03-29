using Do.Architecture;
using Microsoft.AspNetCore.Builder;

namespace Do.Greeting.Swagger;

public class SwaggerGreetingFeature : IFeature<GreetingConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureEndpointRouteBuilder(endpoints =>
        {
            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger/index.html");

                return Task.CompletedTask;
            });
        });
    }
}
