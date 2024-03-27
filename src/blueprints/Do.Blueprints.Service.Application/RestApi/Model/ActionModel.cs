namespace Do.RestApi.Model;

public record ActionModel(
    string Name,
    HttpMethod Method,
    string Route,
    ReturnModel Return,
    string FindTargetStatement,
    string InvokedMethodName
)
{
    public string Name { get; set; } = Name;
    public HttpMethod Method { get; set; } = Method;
    public string Route { get; set; } = Route;
    public ReturnModel Return { get; set; } = Return;
    public string FindTargetStatement { get; set; } = FindTargetStatement;
    public string InvokedMethodName { get; set; } = InvokedMethodName;

    public Dictionary<string, ParameterModel> Parameter { get; init; } = [];

    public bool HasRequestBody => BodyParameters.Any();
    public IEnumerable<ParameterModel> Parameters { get => Parameter.Values; init => Parameter = value.ToDictionary(p => p.Name); }
    public IEnumerable<ParameterModel> BodyParameters => Parameters.Where(p => p.From == ParameterModelFrom.Body);
    public IEnumerable<ParameterModel> NonBodyParameters => Parameters.Where(p => p.From != ParameterModelFrom.Body);
    public IEnumerable<ParameterModel> InvokedMethodParameters => Parameters.Where(p => p.From != ParameterModelFrom.Services);
}
