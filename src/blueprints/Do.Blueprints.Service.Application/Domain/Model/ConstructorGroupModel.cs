namespace Do.Domain.Model;

public record ConstructorGroupModel(
    List<ConstructorModel> Constructors
) : IModel
{
    public string Name { get; } = ".ctor";

    string IModel.Id => Name;
}