using Do.Architecture;
using Do.Blueprints.Service.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Do;

public static class DependencyInjectionExtensions
{
    public static void AddDependencyInjection(this ICollection<ILayer> layers) => layers.Add(new DependencyInjectionLayer());
    public static void ConfigureServiceCollection(this LayerConfigurator configurator, Action<IServiceCollection> configuration) => configurator.Configure(configuration);
}
