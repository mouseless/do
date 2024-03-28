using System.Diagnostics.CodeAnalysis;

namespace Do.Domain.Model;

public class ModelCollection<T> : IEnumerable<T>, IIndexedCollection<T>
    where T : IModel
{
    readonly ModelKeyedCollection<T> _models = [];
    readonly ModelIndex<T> _index = [];

    public ModelCollection() { }

    public ModelCollection(IEnumerable<T> models)
    {
        foreach (var model in models)
        {
            _models.Add(model);
        }
    }

    ModelCollection(IEnumerable<T> models, ModelIndex<T> index)
    {
        foreach (var model in models)
        {
            _models.Add(model);
        }

        _index = index;
    }

    public T this[string id] =>
        _models[id];

    public T this[Type type] =>
        _models[TypeModel.IdFrom(type)];

    public int Count => _models.Count;

    public bool ContainsModel(T? model) =>
        _models.Contains(model?.Id ?? string.Empty);

    public bool Contains(string id) =>
        _models.Contains(id);

    public bool TryGetValue(string id, [NotNullWhen(true)] out T? model) =>
       _models.TryGetValue(id, out model);

    public ModelCollection<T> GetIndex(object key) =>
        _index.TryGetValue(new(key), out var result) ? result : new([]);

    public ModelCollection<T> Having<TAttribute>() where TAttribute : Attribute =>
        GetIndex(typeof(TAttribute));

    public ModelCollection<T> CreateReferenceModelCollection(Func<T, bool> selector) =>
       new(_models.Where(selector), _index);

    public IEnumerator<T> GetEnumerator() => _models.GetEnumerator();

    ModelIndex<T> IIndexedCollection<T>.Index => _index;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
