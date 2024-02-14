﻿using Do.Testing;
using Moq;
using Newtonsoft.Json;
using System.Reflection;

namespace Do.Communication.Mock;

public class MockClientBuilder
{
    static readonly MethodInfo _setupClient = typeof(MockClientBuilder).GetMethod(nameof(SetupClient), BindingFlags.Static | BindingFlags.NonPublic) ??
        throw new("SetupClient<T> should have existed");

    static void SetupClient<T>(Mock<IClient<T>> mock, List<(object? response, Func<Request, bool> when)> setups)
        where T : class
    {
        foreach (var (response, when) in setups)
        {
            mock.Setup(c => c.Send(It.Is<Request>(r => when(r)))).ReturnsAsync(new Response(JsonConvert.SerializeObject(response)));
        }
    }

    readonly Dictionary<Type, List<(object? response, Func<Request, bool> when)>> _list = [];

    public void ForClient<T>(object response) where T : class => ForClient<T>(response, _ => true);
    public void ForClient<T>(object response, Func<Request, bool> when) where T : class
    {
        if (_list.TryGetValue(typeof(T), out var setups))
        {
            setups.Add((response, when));

            return;
        }

        _list[typeof(T)] = [(response, when)];
    }

    internal IMockCollection Build()
    {
        IMockCollection result = new MockCollection();

        foreach (var (type, setups) in _list)
        {
            result.Add(
                service: typeof(IClient<>).MakeGenericType(type),
                singleton: true,
                setup: mock => _setupClient.MakeGenericMethod(type).Invoke(null, [mock, setups])
            );
        }

        return result;
    }
}
