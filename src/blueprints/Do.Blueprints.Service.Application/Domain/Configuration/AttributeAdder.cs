using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class AttributeAdder : IConventionComponent<AttributeAdder>
{
    static AttributeAdder IConventionComponent<AttributeAdder>.New() =>
        new();

    public void Add(IMemberModel model, Attribute attribute)
    {
        model.CustomAttributes.Add(attribute);
    }
}
