namespace Do.Domain.Model;

public record PropertyModel(
    string Name,
    TypeModel PropertyType,
    bool IsPublic,
    bool IsVirtual,
    AttributeCollection CustomAttributes
) : IModel
{
    public bool Has<T>() where T : Attribute =>
        CustomAttributes.ContainsKey<T>();

    string IModel.Id { get; } = Name;
}
