using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class DomainConventionCollection
{
    public ModelConventionCollection<TypeModel> Type { get; } = [];
    public ModelConventionCollection<MethodGroupModel> MethodGroup { get; } = [];
    public ModelConventionCollection<ParameterModel> Parameter { get; } = [];
    public ModelConventionCollection<PropertyModel> Property { get; } = [];
}
