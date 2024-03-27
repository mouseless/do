using System.Reflection;

namespace Do.RestApi.Model;

public record ApiModel
{
    public Dictionary<string, Assembly> Reference { get; init; } = [];
    public Dictionary<string, ControllerModel> Controller { get; init; } = [];

    public IEnumerable<Assembly> References { get => Reference.Values; init => Reference = value.ToDictionary(a => a.GetName().FullName); }
    public IEnumerable<ControllerModel> Controllers { get => Controller.Values; init => Controller = value.ToDictionary(c => c.Name); }
}
