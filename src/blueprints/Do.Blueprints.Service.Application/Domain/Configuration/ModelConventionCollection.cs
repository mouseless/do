using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class ModelConventionCollection<T>() : List<IModelConvention>
    where T : IModel
{ }