using Do.Domain.Model;

namespace Do.Domain.Configuration;

public interface IDomainConfiguration
{
    IModelCollectionConfigurer<TypeModel> Type { get; }
    IModelCollectionConfigurer<MethodGroupModel> MethodGroup { get; }
    IModelCollectionConfigurer<ParameterModel> Parameter { get; }
    IModelCollectionConfigurer<PropertyModel> Property { get; }
}
