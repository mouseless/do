﻿using Do.Architecture;
using Do.Communication;
using Do.HttpServer;
using Do.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Text;

namespace Do;

public static class HttpServerExtensions
{
    public static void AddHttpServer(this List<ILayer> source) => source.Add(new HttpServerLayer());

    public static WebApplicationBuilder GetWebApplicationBuilder(this ApplicationContext source) => source.Get<WebApplicationBuilder>();
    public static ConfigurationManager GetConfigurationManager(this ApplicationContext source) => source.Get<ConfigurationManager>();
    public static WebApplication GetWebApplication(this ApplicationContext source) => source.Get<WebApplication>();

    public static void ConfigureMiddlewareCollection(this LayerConfigurator configurator, Action<IMiddlewareCollection> configuration) => configurator.Configure(configuration);
    public static void ConfigureEndpointRouteBuilder(this LayerConfigurator configurator, Action<IEndpointRouteBuilder> configuration) => configurator.Configure(configuration);

    public static void Add<T>(this IMiddlewareCollection source, int order = default) => source.Add(new(app => app.UseMiddleware<T>(), order));
    public static void Add(this IMiddlewareCollection source, Action<IApplicationBuilder> configure, int order = default) => source.Add(new(configure, order));

    public static T GetRequiredServiceUsingRequestServices<T>(this IServiceProvider source) where T : notnull => (T)source.GetRequiredServiceUsingRequestServices(typeof(T));
    public static object GetRequiredServiceUsingRequestServices(this IServiceProvider source, Type serviceType)
    {
        var http = source.GetRequiredService<IHttpContextAccessor>();

        if (http.HttpContext is null) { return source.GetRequiredService(serviceType); }

        return http.HttpContext.RequestServices.GetRequiredService(serviceType);
    }

    public static HttpRequest AnHttpRequest(this Stubber giveMe,
        string? schema = default,
        string? host = default,
        string? path = default,
        string? method = default,
        Dictionary<string, string>? query = default,
        Dictionary<string, string>? header = default,
        Dictionary<string, string>? form = default,
        object? body = default,
        object[]? metadata = default
    )
    {
        schema ??= "http";
        host ??= "test.com";
        path ??= string.Empty;
        method ??= form is null && body is null ? "GET" : "POST";

        var result = giveMe.AnHttpContext(metadata: metadata).Request;

        result.Scheme = schema;
        result.Host = new(host);
        result.Path = path;
        result.Method = method;

        if (query is not null)
        {
            result.QueryString = new($"?{query.ToQueryString()}");
        }

        if (header is not null)
        {
            foreach (var (key, value) in header)
            {
                result.Headers.Append(key, value);
            }
        }

        if (form is not null)
        {
            result.ContentType = "application/x-www-form-urlencoded";
            result.Form = new FormCollection(form.ToDictionary(kvp => kvp.Key, kvp => new StringValues(kvp.Value)));
        }

        if (body is not null)
        {
            result.ContentType = "application/json";
            result.Body = new MemoryStream(Encoding.UTF8.GetBytes(body.ToJsonString()));
        }

        return result;
    }

    public static HttpContext AnHttpContext(this Stubber giveMe,
        object[]? metadata = default
    )
    {
        var features = new FeatureCollection();
        features.Set(giveMe.Spec.MockMe.AnEndpointFeature(metadata: metadata));
        features.Set<IHttpRequestFeature>(new HttpRequestFeature());

        return new DefaultHttpContext(features);
    }

    public static IEndpointFeature AnEndpointFeature(this Mocker mockMe,
        object[]? metadata = default
    )
    {
        metadata ??= [];

        var mock = new Mock<IEndpointFeature>();
        var endpoint = new Endpoint(mockMe.Spec.GiveMe.ARequestDelegate(), new(metadata), mockMe.Spec.GiveMe.AString());

        mock.Setup(ef => ef.Endpoint).Returns(endpoint);

        return mock.Object;
    }

    public static RequestDelegate ARequestDelegate(this Stubber _) =>
        _ => Task.CompletedTask;
}
