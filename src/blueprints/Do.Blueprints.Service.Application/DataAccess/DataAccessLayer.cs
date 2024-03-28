using Do.Architecture;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

using static Do.DependencyInjection.DependencyInjectionLayer;
using NHConfiguration = NHibernate.Cfg.Configuration;

namespace Do.DataAccess;

public class DataAccessLayer : LayerBase<AddServices>
{
    readonly PersistenceConfiguration _persistenceConfiguration = new();
    readonly InterceptorConfiguration _interceptorConfiguration = new();
    readonly AutomappingConfiguration _automappingConfiguration = new();
    readonly AutoPersistenceModel _autoPersistenceModel;

    volatile bool _exported = false;
    readonly object _exportedLock = new();

    public DataAccessLayer() =>
        _autoPersistenceModel = new(new DelegatedAutomappingConfiguration(_automappingConfiguration));

    protected override PhaseContext GetContext(AddServices phase)
    {
        var services = Context.GetServiceCollection();

        services.AddSingleton<NHConfiguration>(sp =>
        {
            var builder = Fluently.Configure()
                .Database(_persistenceConfiguration.Configurer)
                .ExposeConfiguration(c => c.SetInterceptor(new DelegatedInterceptor(sp, _interceptorConfiguration)))
                .Mappings(m => m.AutoMappings.Add(_autoPersistenceModel));

            if (_persistenceConfiguration.AutoUpdateSchema)
            {
                builder.ExposeConfiguration(c => new SchemaUpdate(c).Execute(true, true));
            }

            return builder.BuildConfiguration();
        });

        services.AddSingleton<ISessionFactory>(sp => sp.GetRequiredService<NHConfiguration>().BuildSessionFactory());
        services.AddSingleton<Func<ISession>>(sp => () => sp.GetRequiredServiceUsingRequestServices<ISession>());

        services.AddScoped<ISession>(sp =>
        {
            var result = sp.GetRequiredService<ISessionFactory>().OpenSession();

            if (_persistenceConfiguration.AutoExportSchema)
            {
                if (!_exported)
                {
                    lock (_exportedLock)
                    {
                        if (!_exported)
                        {
                            var export = new SchemaExport(sp.GetRequiredService<NHConfiguration>());

                            export.Execute(false, true, false, result.Connection, null);
                            _exported = true;
                        }
                    }
                }
            }

            return result;
        });

        return phase.CreateContextBuilder()
            .Add(_persistenceConfiguration)
            .Add(_interceptorConfiguration)
            .Add(_automappingConfiguration)
            .Add(_autoPersistenceModel)
            .Build();
    }
}
