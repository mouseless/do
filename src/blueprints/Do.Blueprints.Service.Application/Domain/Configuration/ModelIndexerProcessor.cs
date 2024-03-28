using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class ModelIndexerProcessor<T>(DomainIndexerCollection _indexers) : IModelCollectionConfigurer<T>
     where T : IModel
{
    void Apply(ModelCollection<T> collection)
    {
        var models = new List<T>();

        foreach (var indexer in _indexers)
        {
            foreach (var model in collection)
            {
                if (indexer.AppliestTo(model))
                {
                    models.Add(model);
                }
            }

            indexer.Apply(collection, models);
            models.Clear();
        }
    }

    void IModelCollectionConfigurer<T>.Apply(ModelCollection<T> collection) => Apply(collection);
}

