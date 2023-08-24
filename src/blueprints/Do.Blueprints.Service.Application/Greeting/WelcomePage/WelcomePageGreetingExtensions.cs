﻿using Do.Greeting;
using Do.Greeting.WelcomePage;

namespace Do;

public static class WelcomePageGreetingExtensions
{
    public static IGreetingFeature WelcomePage(this GreetingConfigurator _,
        string? path = default
    ) => new WelcomePageGreetingFeature(path ?? "/");
}
