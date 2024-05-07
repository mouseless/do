using Do.RestApi.Configuration;
using System.Net;

namespace Do.CodingStyle.UriReturnIsRedirect;

public class UriReturnIsRedirectConvention : IApiModelConvention<ActionModelContext>, IApiModelConvention<ApiModelContext>
{
    public void Apply(ActionModelContext context)
    {
        if (context.Action.MappedMethod is null) { return; }
        if (!context.Action.Return.TypeModel.Is<Uri>(allowAsync: true)) { return; }

        context.Action.AdditionalAttributes.Add($"ProducesResponseType((int){typeof(HttpStatusCode).Name}.Redirect)");
        context.Action.Return.Type = context.Action.Return.IsAsync ? "Task<RedirectResult>" : "RedirectResult";
        context.Action.Return.ResultRenderer = resultExpression => $"new RedirectResult($\"{{{resultExpression}}}\", permanent: false)";
    }

    public void Apply(ApiModelContext context)
    {
        context.Api.Usings.Add("System.Net");
    }
}