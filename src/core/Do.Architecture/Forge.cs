﻿using Do.Architecture;
using Do.Branding;

namespace Do;

public class Forge
{
    public static Forge New => new(new DoBanner(), () => new(new()));

    readonly IBanner _banner;
    readonly Func<Application> _newApplication;

    public Forge(IBanner banner, Func<Application> newApplication)
    {
        _banner = banner;
        _newApplication = newApplication;
    }

    public Application Application(Action<ApplicationDescriptor> describe)
    {
        _banner.Print();

        var descriptor = new ApplicationDescriptor();

        describe(descriptor);

        return _newApplication().With(descriptor);
    }
}
