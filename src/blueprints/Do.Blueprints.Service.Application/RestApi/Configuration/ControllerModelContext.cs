using Do.RestApi.Model;

namespace Do.RestApi.Configuration;

public record ControllerModelContext : ApiModelContext
{
    public required ControllerModel Controller { get; init; }
}
