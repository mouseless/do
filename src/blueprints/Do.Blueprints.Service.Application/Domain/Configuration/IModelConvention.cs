using Do.Domain.Model;

namespace Do.Domain.Configuration;

public interface IModelConvention
{
    int Order { get; }
    bool AppliesTo(IModel model);
    void Apply(IModel model);
}
