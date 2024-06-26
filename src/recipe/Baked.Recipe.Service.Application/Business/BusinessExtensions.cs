﻿using Baked.Architecture;
using Baked.Business;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;

namespace Baked;

public static class BusinessExtensions
{
    public static void AddBusiness(this List<IFeature> features, Func<BusinessConfigurator, IFeature<BusinessConfigurator>> configure) =>
        features.Add(configure(new()));

    static readonly MethodInfo _addTransientWithFactory = typeof(BusinessExtensions).GetMethod(nameof(AddTransientWithFactory), 2, [typeof(IServiceCollection)]) ??
        throw new("AddTransientWithFactory<TService, TImplementation> should have existed");

    static readonly MethodInfo _addScopedWithFactory = typeof(BusinessExtensions).GetMethod(nameof(AddScopedWithFactory), 2, [typeof(IServiceCollection)]) ??
        throw new Exception("AddScopedWithFactory<TService, TImplementation> should have existed");

    public static IServiceCollection AddTransientWithFactory(this IServiceCollection services, Type service) =>
        (IServiceCollection?)_addTransientWithFactory.MakeGenericMethod(service, service).Invoke(null, [services]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static IServiceCollection AddTransientWithFactory(this IServiceCollection services, Type service, Type implementation) =>
        (IServiceCollection?)_addTransientWithFactory.MakeGenericMethod(service, implementation).Invoke(null, [services]) ??
        throw new("Should've returned an IServiceCollection instance");

    public static IServiceCollection AddTransientWithFactory<TService>(this IServiceCollection services) where TService : class =>
        services.AddTransientWithFactory<TService, TService>();

    public static IServiceCollection AddTransientWithFactory<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services
        .AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>())
        .AddTransient<TService, TImplementation>();

    public static IServiceCollection AddScopedWithFactory(this IServiceCollection services, Type service) =>
        (IServiceCollection?)_addScopedWithFactory.MakeGenericMethod(service, service).Invoke(null, [services]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static IServiceCollection AddScopedWithFactory(this IServiceCollection services, Type service, Type implementation) =>
        (IServiceCollection?)_addScopedWithFactory.MakeGenericMethod(service, implementation).Invoke(null, [services]) ??
        throw new("Should've returned an IServiceCollection instance");
    public static void AddScopedWithFactory<TService>(this IServiceCollection services) where TService : class =>
        services.AddScopedWithFactory<TService, TService>();

    public static IServiceCollection AddScopedWithFactory<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services
        .AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>())
        .AddScoped<TService, TImplementation>();

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, bool forward)
        where TService : class
        where TImplementation : class, TService
    => services.AddSingleton(typeof(TService), typeof(TImplementation), forward: forward);

    public static IServiceCollection AddSingleton(this IServiceCollection services, Type service, Type implementation, bool forward)
    {
        if (!forward) { return services.AddSingleton(service, implementation); }

        return services.AddSingleton(service, sp => sp.GetRequiredServiceUsingRequestServices(implementation));
    }

    public static bool TryGetMappedMethod(this ApiDescription apiDescription, [NotNullWhen(true)] out MappedMethodAttribute? result)
    {
        result = apiDescription.CustomAttributes().OfType<MappedMethodAttribute>().SingleOrDefault();

        return result is not null;
    }

    public static void SetJsonExample(this IDictionary<string, OpenApiMediaType> mediaTypes, XmlNode? documentation, string @for)
    {
        if (!mediaTypes.TryGetValue("application/json", out var mediaType)) { return; }

        var example = documentation.GetExampleCode(@for);
        if (example is null) { return; }

        mediaType.Example = OpenApiAnyFactory.CreateFromJson(example);
    }
}