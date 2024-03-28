using Do.Architecture;
using Do.Business;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Do;

public static class BusinessExtensions
{
    public static void AddBusiness(this List<IFeature> source, Func<BusinessConfigurator, IFeature<BusinessConfigurator>> configure) => source.Add(configure(new()));

    static readonly MethodInfo _addTransientWithFactory = typeof(BusinessExtensions).GetMethod(nameof(AddTransientWithFactory), 2, [typeof(IServiceCollection)]) ??
        throw new("AddTransientWithFactory<TService, TImplementation> should have existed");

    static readonly MethodInfo _addScopedWithFactory = typeof(BusinessExtensions).GetMethod(nameof(AddScopedWithFactory), 2, [typeof(IServiceCollection)]) ??
        throw new Exception("AddScopedWithFactory<TService, TImplementation> should have existed");

    public static IServiceCollection AddTransientWithFactory(this IServiceCollection source, Type service) =>
        (IServiceCollection?)_addTransientWithFactory.MakeGenericMethod(service, service).Invoke(null, [source]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static IServiceCollection AddTransientWithFactory(this IServiceCollection source, Type service, Type implementation) =>
        (IServiceCollection?)_addTransientWithFactory.MakeGenericMethod(service, implementation).Invoke(null, [source]) ??
        throw new("Should've returned an IServiceCollection instance");

    public static IServiceCollection AddTransientWithFactory<TService>(this IServiceCollection source) where TService : class =>
        source.AddTransientWithFactory<TService, TService>();

    public static IServiceCollection AddTransientWithFactory<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    => source
        .AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>())
        .AddTransient<TService, TImplementation>();

    public static IServiceCollection AddScopedWithFactory(this IServiceCollection source, Type service) =>
        (IServiceCollection?)_addScopedWithFactory.MakeGenericMethod(service, service).Invoke(null, [source]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static IServiceCollection AddScopedWithFactory(this IServiceCollection source, Type service, Type implementation) =>
        (IServiceCollection?)_addScopedWithFactory.MakeGenericMethod(service, implementation).Invoke(null, [source]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static void AddScopedWithFactory<TService>(this IServiceCollection source) where TService : class =>
        source.AddScopedWithFactory<TService, TService>();

    public static IServiceCollection AddScopedWithFactory<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    => source
        .AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>())
        .AddScoped<TService, TImplementation>();

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection source, bool forward)
        where TService : class
        where TImplementation : class, TService
    => source.AddSingleton(typeof(TService), typeof(TImplementation), forward: forward);

    public static IServiceCollection AddSingleton(this IServiceCollection source, Type service, Type implementation, bool forward)
    {
        if (!forward) { return source.AddSingleton(service, implementation); }

        return source.AddSingleton(service, sp => sp.GetRequiredServiceUsingRequestServices(implementation));
    }
}
