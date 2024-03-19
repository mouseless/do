using System.Diagnostics.CodeAnalysis;

namespace Do.Domain.Model;

public class ModelCollection<T>() : IEnumerable<T>
    where T : IModel
{
    readonly KeyedModelCollection<T> _models = [];

    public ModelCollection(IEnumerable<T> models)
        : this()
    {
        foreach (var model in models)
        {
            _models.Add(model);
        }
    }

    public T this[string id] =>
        _models[id];

    public bool ContainsModel(T? model) =>
        _models.Contains(model?.Id ?? string.Empty);

    public bool Contains(string id) =>
        _models.Contains(id);

    public bool TryGetValue(string id, [NotNullWhen(true)] out T? model) =>
       _models.TryGetValue(id, out model);

    public IEnumerator<T> GetEnumerator() => _models.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
