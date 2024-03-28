using Do.Domain.Model;

namespace Do.Domain.Configuration;

internal class DomainModelBuilder(DomainModelBuilderOptions _options)
{
    readonly TypeModelBuildQueue _buildQueue = new();
    readonly HashSet<Type> _domainTypes = [];
    readonly ModelKeyedCollection<TypeModel> _types = [];

    public DomainModelBuilderOptions Options => _options;

    public DomainModel BuildFrom(IDomainTypeCollection types)
    {
        foreach (var type in types)
        {
            _domainTypes.Add(type);
        }

        _buildQueue.EnqueueAll(types);

        do
        {
            var currentTypes = _buildQueue.DequeueAll();
            foreach (var type in currentTypes)
            {
                _types.Add(type);
            }

            foreach (var typeModel in currentTypes)
            {
                typeModel.Apply(t =>
                {
                    var buildLevel = GetBuildLevel(t);
                    foreach (var level in BuildLevel.All.Where(bl => buildLevel.Covers(bl)))
                    {
                        level.Set(typeModel, t, this);
                    }
                });
            }
        }
        while (!_buildQueue.IsEmpty);

        return new(new(_types));
    }

    BuildLevel GetBuildLevel(Type t)
    {
        var context = new TypeBuildContext(t, this);

        return _options.BuildLevels.FirstOrDefault(blf => blf.Filter(context))?.BuildLevel ?? BuildLevel.Basics;
    }

    internal bool IsDomainType(Type t) =>
        _domainTypes.Contains(t);

    public TypeModel Get(Type type)
    {
        if (_types.TryGetValue(TypeModel.IdFrom(type), out var result))
        {
            return result;
        }

        return _buildQueue.Enqueue(type);
    }
}
