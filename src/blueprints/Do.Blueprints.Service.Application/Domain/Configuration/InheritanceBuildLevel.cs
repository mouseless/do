using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class InheritanceBuildLevel : BuildLevel
{
    internal override void Set(TypeModel typeModel, Type type, DomainModelBuilder builder) =>
        typeModel.SetInheritance(
            type.BaseType is null ? default : builder.Get(type.BaseType),
            type.GetInterfaces().Select(builder.Get).ToModelCollection()
        );
}