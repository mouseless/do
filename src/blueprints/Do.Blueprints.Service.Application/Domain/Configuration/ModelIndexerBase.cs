using Do.Domain.Model;

namespace Do.Domain.Configuration;

public abstract class ModelIndexerBase : IModelIndexer
{
    protected abstract ModelIndexKey IndexKey { get; }
    protected abstract bool AppliesTo(IModel model);

    void Apply<T>(ModelIndex<T> index, IEnumerable<T> models) where T : IModel
        => index[IndexKey] = new(models);

    bool IModelIndexer.AppliestTo(IModel model) => AppliesTo(model);
    void IModelIndexer.Apply<T>(IIndexedCollection<T> collection, IEnumerable<T> models) => Apply(collection.Index, models);
}
