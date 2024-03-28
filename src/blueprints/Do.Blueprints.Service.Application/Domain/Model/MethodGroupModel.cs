
namespace Do.Domain.Model;

public record MethodGroupModel(
    string Name,
    List<MethodModel> Methods
) : IModel
{
    string IModel.Id => Name;
}
