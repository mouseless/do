namespace Do.RestApi.Model;

public record ParameterModel(ParameterModelFrom From, string Type, string Name)
{
    public ParameterModelFrom From { get; set; } = From;
    public string Type { get; set; } = Type;
    public string Name { get; set; } = Name;
    public string InternalName { get; set; } = Name;
    public Func<string, string> RenderLookup { get; set; } = parameterExpression => parameterExpression;

    public bool FromBody => From == ParameterModelFrom.Body;
}
