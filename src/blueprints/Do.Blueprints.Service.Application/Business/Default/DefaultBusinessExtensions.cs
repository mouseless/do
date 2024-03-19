using Do.Business;
using Do.Business.Default;
using System.Reflection;

namespace Do;

public static class DefaultBusinessExtensions
{
    public static DefaultBusinessFeature Default(this BusinessConfigurator _, List<Assembly> assemblies) =>
        new(assemblies);
}
