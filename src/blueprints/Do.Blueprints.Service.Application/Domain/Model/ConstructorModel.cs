namespace Do.Domain.Model;

public record ConstructorModel(
    string Name,
    bool IsPublic,
    bool IsProtected,
    AttributeCollection CustomAttributes,
    ModelCollection<ParameterModel> Parameters
) : MethodBaseModel(
    Name,
    IsPublic,
    IsProtected,
    false,
    true,
    default,
    CustomAttributes,
    Parameters
);
