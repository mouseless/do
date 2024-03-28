namespace Do.Domain.Model;

public class ModelIndex<T> : Dictionary<ModelIndexKey, ModelCollection<T>> where T : IModel
{ }

public class ModelIndexKey(object key) : IEquatable<ModelIndexKey>
{
    readonly object _key = key;

    public bool Equals(ModelIndexKey? other) =>
        other is not null && other.GetHashCode() == GetHashCode();

    public override bool Equals(object? obj) =>
        Equals(obj);

    public override int GetHashCode() =>
        _key.GetHashCode();
}
