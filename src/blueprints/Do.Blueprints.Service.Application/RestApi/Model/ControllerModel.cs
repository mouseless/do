namespace Do.RestApi.Model;

public record ControllerModel(string Name)
{
    public string Name { get; set; } = Name;
    public Dictionary<string, ActionModel> Action { get; init; } = [];

    public IEnumerable<ActionModel> Actions { get => Action.Values; init => Action = value.ToDictionary(a => a.Name); }
}
