using Do.Domain.Model;

namespace Do.Test;

public abstract class TestServiceSpec : ServiceSpec
{
    protected static DomainModel DomainModel { get; private set; } = default!;

    static TestServiceSpec()
    {
        var context = Init(
            business: c => c.Default(assemblies: [typeof(Entity).Assembly]),
            communication: c => c.Mock(defaultResponses: response =>
            {
                response.ForClient<Singleton>(response: "test result");
                response.ForClient<Operation>(response: "path1 response", when: r => r.UrlOrPath.Equals("path1"));
                response.ForClient<Operation>(response: "path2 response", when: r => r.UrlOrPath.Equals("path2"));
            }),
            configure: app =>
            {
                app.Features.AddConfigurationOverrider();
            }
        );

        DomainModel = context.GetDomainModel();
    }

    protected override string? GetDefaultSettingsValue(string key) =>
        key == "Int" ? "42" : "test value";
}