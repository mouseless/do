using Do.Test;

Forge.New
    .Service(
        authentication: c => c.FixedToken(),
        business: c => c.Default(assemblies: [typeof(Entity).Assembly]),
        database: c => c.MySql().ForDevelopment(c.Sqlite()),
        exceptionHandling: ex => ex.Default(typeUrlFormat: "https://do.mouseless.codes/errors/{0}"),
        configure: app => app.Features.AddConfigurationOverrider()
    )
    .Run();
