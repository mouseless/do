using Do.Domain.Model;

namespace Do.Domain.Configuration;

public class AttributeIndexer : ModelIndexerBase
{
    public static AttributeIndexer For<T>() where T : Attribute
        => new(typeof(T));
    public static AttributeIndexer For<T>(T attribute) where T : Attribute
        => new(typeof(T), attribute);

    readonly Type _attributeType;
    readonly Attribute? _attribute;
    readonly ModelIndexKey _modelIndexKey;

    AttributeIndexer(Type attributeType,
        Attribute? attribute = default
    )
    {
        _attributeType = attributeType;
        _attribute = attribute;
        _modelIndexKey = new(_attribute is null ? _attributeType : _attribute);
    }

    protected override ModelIndexKey IndexKey =>
        _modelIndexKey;

    protected override bool AppliesTo(IModel model) =>
        model is IMemberModel member &&
        (_attribute is not null ? member.CustomAttributes.Contains(_attribute) : member.CustomAttributes.ContainsKey(_attributeType));
}
