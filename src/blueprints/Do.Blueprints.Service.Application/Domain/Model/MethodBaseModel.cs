namespace Do.Domain.Model;

public abstract record MethodBaseModel(
    string Name,
    bool IsPublic,
    bool IsProtected,
    bool IsVirtual,
    bool IsConstructor,
    TypeModel? ReturnType,
    AttributeCollection CustomAttributes,
    ModelCollection<ParameterModel> Parameters
) : IMemberModel
{
    string IModel.Id { get; } = $"{ReturnType?.Name}{Name}({string.Join(", ", Parameters.Select(p => p.Name))})";
}