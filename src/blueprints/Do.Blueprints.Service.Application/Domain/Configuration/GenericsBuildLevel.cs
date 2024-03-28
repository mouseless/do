using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class GenericsBuildLevel : BuildLevel
{
    internal override void Set(TypeModel typeModel, Type type, DomainModelBuilder builder)
    {
        if (type.IsGenericType)
        {
            typeModel.SetGenerics(
                !type.IsGenericTypeDefinition ? builder.Get(type.GetGenericTypeDefinition()) : default,
                type.GenericTypeArguments.Select(builder.Get).ToModelCollection()
            );
        }
        else
        {
            typeModel.SetGenerics(default, []);
        }
    }
}
