using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class TypeModelBuildQueue
{
    readonly ModelKeyedCollection<TypeModel> _data = [];

    public bool IsEmpty => _data.Count == 0;

    public void EnqueueAll(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            Enqueue(type);
        }
    }

    public TypeModel Enqueue(Type type)
    {
        if (_data.TryGetValue(TypeModel.IdFrom(type), out var result)) { return result; }

        _data.Add(result = new(type));

        return result;
    }

    public List<TypeModel> DequeueAll()
    {
        var result = new List<TypeModel>(_data);

        _data.Clear();

        return result;
    }
}
