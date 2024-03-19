using Do.Architecture;
using Do.Business;
using Do.Database;
using Do.Testing;

namespace Do.Test;

public abstract class TestServiceNfr : ServiceNfr<TestServiceNfr>, IEntryPoint
{
    public static void Main(string[] args) => Init(args);

    protected override Func<BusinessConfigurator, IFeature<BusinessConfigurator>> Business =>
        c => c.Default(assemblies: [typeof(Entity).Assembly]);
    protected override Func<DatabaseConfigurator, IFeature<DatabaseConfigurator>>? Database =>
        c => c.InMemory();
    protected override Action<ApplicationDescriptor>? Configure =>
        app => app.Features.AddConfigurationOverrider();
}
