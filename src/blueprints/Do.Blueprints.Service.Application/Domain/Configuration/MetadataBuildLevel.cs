using Do.Domain.Model;
using System.Reflection;

namespace Do.Domain.Configuration;

public class MetadataBuildLevel : BuildLevel
{
    internal override void Set(TypeModel typeModel, Type type, DomainModelBuilder builder) =>
        typeModel.SetMetadata(new(type.GetCustomAttributes()));
}
