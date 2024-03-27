using Do.RestApi.Model;

namespace Do.RestApi.Configuration;

public record ApiModelContext
{
    public required ApiModel Api { get; init; }
}
