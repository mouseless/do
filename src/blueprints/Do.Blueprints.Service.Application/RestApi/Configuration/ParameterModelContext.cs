using ParameterModel = Do.RestApi.Model.ParameterModel;

namespace Do.RestApi.Configuration;

public record ParameterModelContext : ActionModelContext
{
    public required ParameterModel Parameter { get; init; }
}
