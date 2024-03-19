using Do.Architecture;
using Do.Authentication;
using Do.Business;
using Do.Caching;
using Do.Communication;
using Do.Core;
using Do.Database;
using Do.Documentation;
using Do.ExceptionHandling;
using Do.Greeting;
using Do.Logging;
using Do.Orm;

namespace Do;

public static class ForgeExtensions
{
    public static Application Service(this Forge source,
        Func<BusinessConfigurator, IFeature<BusinessConfigurator>> business,
        Func<AuthenticationConfigurator, IFeature<AuthenticationConfigurator>>? authentication = default,
        Func<CachingConfigurator, IFeature<CachingConfigurator>>? caching = default,
        Func<CommunicationConfigurator, IFeature<CommunicationConfigurator>>? communication = default,
        Func<CoreConfigurator, IFeature<CoreConfigurator>>? core = default,
        Func<DatabaseConfigurator, IFeature<DatabaseConfigurator>>? database = default,
        Func<DocumentationConfigurator, IFeature<DocumentationConfigurator>>? documentation = default,
        Func<ExceptionHandlingConfigurator, IFeature<ExceptionHandlingConfigurator>>? exceptionHandling = default,
        Func<GreetingConfigurator, IFeature<GreetingConfigurator>>? greeting = default,
        Func<LoggingConfigurator, IFeature<LoggingConfigurator>>? logging = default,
        Func<OrmConfigurator, IFeature<OrmConfigurator>>? orm = default,
        Action<ApplicationDescriptor>? configure = default
    )
    {
        authentication ??= c => c.Disabled();
        caching ??= c => c.ScopedMemory();
        communication ??= c => c.Http();
        core ??= c => c.Dotnet();
        database ??= c => c.Sqlite();
        documentation ??= c => c.Default();
        exceptionHandling ??= c => c.Default();
        greeting ??= c => c.Swagger();
        logging ??= c => c.RequestLogging();
        orm ??= c => c.Default();
        configure ??= _ => { };

        return source.Application(app =>
        {
            app.Layers.AddCodeGeneration();
            app.Layers.AddConfiguration();
            app.Layers.AddDataAccess();
            app.Layers.AddDependencyInjection();
            app.Layers.AddDomain();
            app.Layers.AddHttpClient();
            app.Layers.AddHttpServer();
            app.Layers.AddMonitoring();
            app.Layers.AddRestApi();

            app.Features.AddAuthentication(authentication);
            app.Features.AddBusiness(business);
            app.Features.AddCaching(caching);
            app.Features.AddCommunication(communication);
            app.Features.AddCore(core);
            app.Features.AddDatabase(database);
            app.Features.AddDocumentation(documentation);
            app.Features.AddExceptionHandling(exceptionHandling);
            app.Features.AddGreeting(greeting);
            app.Features.AddLogging(logging);
            app.Features.AddOrm(orm);

            configure(app);
        });
    }
}
