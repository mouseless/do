namespace Do.Domain.Model;

public interface IIndexedCollection<T> where T : IModel
{
    ModelIndex<T> Index { get; }
}
