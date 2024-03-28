using Do.Domain.Model;

namespace Do.Domain.Configuration;

public interface IModelIndexer
{
    bool AppliestTo(IModel model);
    void Apply<T>(IIndexedCollection<T> index, IEnumerable<T> models) where T : IModel;
}
