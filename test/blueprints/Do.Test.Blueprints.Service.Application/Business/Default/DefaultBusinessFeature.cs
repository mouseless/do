﻿using Do.Architecture;
using Do.Business;
using System.Reflection;

namespace Do.Test.Business.Default;

public class DefaultBusinessFeature : IFeature<BusinessConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureServiceCollection(services =>
        {
            services.AddTransientWithFactory<Entity>();
            services.AddSingleton<Entities>();
            services.AddSingleton<Singleton>();
        });

        configurator.ConfigureAutoPersistenceModel(model =>
        {
            model.AddEntityAssembly(typeof(Entity).Assembly);
        });

        configurator.ConfigureApplicationParts(applicationParts =>
        {
            applicationParts.Add(new(Assembly.GetEntryAssembly() ?? throw new NotSupportedException("Entry assembly should not be null")));
        });
    }
}