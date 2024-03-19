using Do.Architecture;
using Do.Authentication;
using Do.Business;
using Do.Caching;
using Do.Communication;
using Do.Core;
using Do.Database;
using Do.ExceptionHandling;
using Do.MockOverrider;
using Do.Orm;
using Do.Testing;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

using ITransaction = NHibernate.ITransaction;

namespace Do;

public abstract class ServiceSpec : Spec
{
    static IServiceProvider _serviceProvider = default!;

    internal static IServiceProvider ServiceProvider => _serviceProvider;
    internal static ISession Session => _serviceProvider.GetRequiredService<ISession>();

    protected static ApplicationContext Init(
        Func<BusinessConfigurator, IFeature<BusinessConfigurator>> business,
        Func<AuthenticationConfigurator, IFeature<AuthenticationConfigurator>>? authentication = default,
        Func<CachingConfigurator, IFeature<CachingConfigurator>>? caching = default,
        Func<CommunicationConfigurator, IFeature<CommunicationConfigurator>>? communication = default,
        Func<CoreConfigurator, IFeature<CoreConfigurator>>? core = default,
        Func<DatabaseConfigurator, IFeature<DatabaseConfigurator>>? database = default,
        Func<ExceptionHandlingConfigurator, IFeature<ExceptionHandlingConfigurator>>? exceptionHandling = default,
        Func<MockOverriderConfigurator, IFeature<MockOverriderConfigurator>>? mockOverrider = default,
        Func<OrmConfigurator, IFeature<OrmConfigurator>>? orm = default,
        Action<ApplicationDescriptor>? configure = default
    )
    {
        authentication ??= c => c.Disabled();
        caching ??= c => c.ScopedMemory();
        communication ??= c => c.Mock();
        core ??= c => c.Mock();
        database ??= c => c.InMemory();
        exceptionHandling ??= c => c.Default();
        mockOverrider ??= c => c.FirstInterface();
        orm ??= c => c.Default();

        var context = Spec.Init(app =>
        {
            app.Layers.AddCodeGeneration();
            app.Layers.AddConfiguration();
            app.Layers.AddDataAccess();
            app.Layers.AddDependencyInjection();
            app.Layers.AddDomain();
            app.Layers.AddMonitoring();
            app.Layers.AddRestApi();
            app.Layers.AddTesting();

            app.Features.AddAuthentication(authentication);
            app.Features.AddBusiness(business);
            app.Features.AddCaching(caching);
            app.Features.AddCommunication(communication);
            app.Features.AddCore(core);
            app.Features.AddDatabase(database);
            app.Features.AddExceptionHandling(exceptionHandling);
            app.Features.AddMockOverrider(mockOverrider);
            app.Features.AddOrm(orm);

            configure?.Invoke(app);
        });

        _serviceProvider = context.GetServiceProvider();

        return context;
    }

    ITransaction _transaction = default!;
    internal Dictionary<string, string> Settings { get; private set; } = default!;

    public override void SetUp()
    {
        base.SetUp();

        _transaction = Session.BeginTransaction();

        Settings = [];

        MockMe.TheConfiguration(settings: Settings, defaultValueProvider: GetDefaultSettingsValue);

        // This is the initial release date of DO. Do not change this the avoid
        // potential "Cannot go back in time." errors.
        MockMe.TheTime(now: new DateTime(2023, 06, 15, 16, 59, 00), reset: true);
    }

    public override void TearDown()
    {
        base.TearDown();

        Session.Flush();
        _transaction.Rollback();
        Session.Clear();

        GiveMe.The<IMockOverrider>().Reset();
        GiveMe.AMemoryCache(clear: true);
    }

    protected virtual string? GetDefaultSettingsValue(string key) =>
        "test value";
}
