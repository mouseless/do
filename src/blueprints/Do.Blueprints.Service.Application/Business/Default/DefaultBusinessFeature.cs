using Do.Architecture;
using Do.Business.Default.RestApiConventions;
using Do.RestApi.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Do.Business.Default;

public class DefaultBusinessFeature(List<Assembly> _domainAssemblies)
    : IFeature<BusinessConfigurator>
{
    const BindingFlags _defaultMemberBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureDomainAssemblyCollection(assemblies =>
        {
            foreach (var assembly in _domainAssemblies)
            {
                assemblies.Add(assembly);
            }
        });

        configurator.ConfigureDomainBuilderOptions(options =>
        {
            options.ConstuctorBindingFlags = _defaultMemberBindingFlags;
            options.MethodBindingFlags = _defaultMemberBindingFlags;
            options.PropertyBindingFlags = _defaultMemberBindingFlags;
        });

        configurator.ConfigureServiceCollection(services =>
        {
            var domainModel = configurator.Context.GetDomainModel();
            foreach (var type in domainModel.Types.Where(t => !t.IsIgnored()))
            {
                if (type.IsTransient())
                {
                    type.Apply(t =>
                    {
                        services.AddTransientWithFactory(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddTransientWithFactory(i, t));
                    });
                }
                else if (type.IsScoped())
                {
                    type.Apply(t =>
                    {
                        services.AddScopedWithFactory(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddScopedWithFactory(i, t));
                    });
                }
                else if (type.IsSingleton())
                {
                    type.Apply(t =>
                    {
                        services.AddSingleton(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddSingleton(i, t, forward: true));
                    });
                }
            }
        });

        configurator.ConfigureApiModel(api =>
        {
            _domainAssemblies.ForEach(a => api.Reference.Add(a.GetName().FullName, a));

            var domainModel = configurator.Context.GetDomainModel();
            foreach (var type in domainModel.Types.Where(t => !t.IsIgnored()))
            {
                if (type.FullName is null) { continue; }
                if (!type.IsSingleton()) { continue; } // TODO for now only singleton

                var controller = new ControllerModel(type.Name);
                foreach (var method in type.Methods.Where(m => !m.IsConstructor && m.Overloads.Count(o => o.IsPublic) > 0))
                {
                    var overload = method.Overloads.OrderByDescending(o => o.Parameters.Count).First(); // overload with most parameters
                    if (overload.ReturnType is null) { continue; }

                    // TODO for now only primitive, list of primitive and entity parameters
                    if (overload.Parameters.Count(p => !p.ParameterType.IsPrimitive() && !p.ParameterType.IsPrimitiveList() && !p.ParameterType.IsEntity()) > 0) { continue; }
                    if (overload.ReturnType.FullName != typeof(void).FullName &&
                        overload.ReturnType.FullName != typeof(Task).FullName) { continue; } // TODO for now only void

                    controller.Action.Add(
                        method.Name,
                        new(
                            Name: method.Name,
                            Method: HttpMethod.Post,
                            Route: $"generated/{type.Name}/{method.Name}",
                            Return: new(async: overload.ReturnType.FullName == typeof(Task).FullName),
                            FindTargetStatement: "target",
                            InvokedMethodName: method.Name
                        )
                        {
                            Parameters = [
                                new(ParameterModelFrom.Services, type.FullName, "target"),
                                .. overload.Parameters.Select(p => new ParameterModel(ParameterModelFrom.Body, p.ParameterType.CSharpFriendlyFullName, p.Name))
                            ]
                        }
                    );
                }

                api.Controller.Add(controller.Name, controller);
            }
        });

        configurator.ConfigureApiModelConventions(conventions =>
        {
            conventions.Add(new LookupEntityByIdConvention(configurator.Context.GetDomainModel()));
        });

        configurator.ConfigureMvcNewtonsoftJsonOptions(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        });
    }
}
