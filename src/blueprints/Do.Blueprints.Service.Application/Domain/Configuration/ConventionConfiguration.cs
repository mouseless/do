using Do.Domain.Model;

namespace Do.Domain.Configuration;

internal class ConventionConfiguration(DomainConventionCollection _conventions) : IDomainConfiguration
{
    IModelCollectionConfigurer<TypeModel> IDomainConfiguration.Type { get; } = new ModelConventionProcessor<TypeModel>(_conventions.Type);
    IModelCollectionConfigurer<MethodGroupModel> IDomainConfiguration.MethodGroup { get; } = new ModelConventionProcessor<MethodGroupModel>(_conventions.MethodGroup);
    IModelCollectionConfigurer<ParameterModel> IDomainConfiguration.Parameter { get; } = new ModelConventionProcessor<ParameterModel>(_conventions.Parameter);
    IModelCollectionConfigurer<PropertyModel> IDomainConfiguration.Property { get; } = new ModelConventionProcessor<PropertyModel>(_conventions.Property);
}
