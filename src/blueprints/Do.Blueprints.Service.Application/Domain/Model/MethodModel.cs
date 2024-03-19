namespace Do.Domain.Model;

public record MethodModel(
    string Name,
    OverloadModel[] Overloads,
    bool IsConstructor = false
) : IModel
{
    string IModel.Id { get; } = Name;

    public bool CanReturn(TypeModel type) =>
        Overloads.Any(o =>
            o.ReturnType == type ||
            (o.ReturnType?.IsAssignableTo<Task>() == true && o.ReturnType?.GenericTypeArguments.Any(a => a == type) == true)
        );
}
