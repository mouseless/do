using Do.RestApi.Model;

namespace Do.RestApi.Configuration;

public record ActionModelContext : ControllerModelContext
{
    public required ActionModel Action { get; init; }
}
