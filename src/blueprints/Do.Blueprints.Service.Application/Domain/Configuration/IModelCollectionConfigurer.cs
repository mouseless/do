using Do.Domain.Model;

namespace Do.Domain.Configuration;

public interface IModelCollectionConfigurer<T> where T : IModel
{
    void Apply(ModelCollection<T> collection);
}