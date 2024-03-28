using Do.Architecture;
using Do.Domain;
using Do.Domain.Configuration;
using Do.Domain.Model;

namespace Do;

public static class DomainExtensions
{
    public static void AddDomain(this ICollection<ILayer> layers) => layers.Add(new DomainLayer());

    public static DomainModel GetDomainModel(this ApplicationContext source) => source.Get<DomainModel>();

    public static void ConfigureDomainTypeCollection(this LayerConfigurator configurator, Action<IDomainTypeCollection> configuration) => configurator.Configure(configuration);
    public static void ConfigureDomainBuilderOptions(this LayerConfigurator configurator, Action<DomainModelBuilderOptions> configuration) => configurator.Configure(configuration);
    public static void ConfigureDomainMetaData(this LayerConfigurator configurator, Action<DomainConventionCollection> configuration) => configurator.Configure(configuration);
    public static void ConfigureDomainIndexers(this LayerConfigurator configurator, Action<DomainIndexerCollection> configuration) => configurator.Configure(configuration);

    public static void Add<T>(this IDomainTypeCollection source) => source.Add(typeof(T));

    public static void AddAttributeIndexer<T>(this DomainIndexerCollection source) where T : Attribute => source.Add(AttributeIndexer.For<T>());
    public static void AddAttributeIndexer<T>(this DomainIndexerCollection source, T attribute) where T : Attribute => source.Add(AttributeIndexer.For<T>(attribute));

    public static ModelConventionCollection<T> Add<T>(this ModelConventionCollection<T> source, Attribute add, Func<T, bool> when,
        int? order = default
    )
        where T : IMemberModel
    => source.Add((model, adder) => adder.Add(model, add), when, order);

    public static ModelConventionCollection<T> Add<T, TAttribute>(this ModelConventionCollection<T> source, Attribute[] add, Func<T, bool> when,
        int? order = default
    ) where T : IMemberModel
    => source.Add((model, adder) => Array.ForEach(add, a => adder.Add(model, a)), when, order);

    public static ModelConventionCollection<T> Add<T>(this ModelConventionCollection<T> source, Action<T, AttributeAdder> add, Func<T, bool> when,
        int? order = default
    ) where T : IModel
    {
        source.Add(new ModelConvention<T, AttributeAdder>(_apply: add, _appliesTo: when, _order: order ?? 100));

        return source;
    }

    public static void Add(this List<TypeBuildLevelFilter> filters, Func<Type, bool> filter, BuildLevel buildLevel) =>
        filters.Add(context => filter(context.Type), buildLevel);

    public static void Add(this List<TypeBuildLevelFilter> filters, Func<TypeBuildContext, bool> filter, BuildLevel buildLevel) =>
        filters.Add(new(filter, buildLevel));

    public static ModelCollection<TModel> ToModelCollection<TModel>(this IEnumerable<TModel> models) where TModel : IModel =>
        new(models);

    public static void Apply(this IEnumerable<TypeModel> types, Action<Type> action)
    {
        foreach (var type in types)
        {
            type.Apply(action);
        }
    }

    public static bool Contains(this ModelCollection<TypeModel> source, Type type) =>
        source.Contains(TypeModel.IdFrom(type));
}
