using Do.CodeGeneration;
using Do.RestApi.Model;

namespace Do.RestApi;

public class ApiCodeTemplate(ApiModel _apiModel)
    : CodeTemplateBase
{
    protected override IEnumerable<string> Render() =>
        _apiModel.Controllers.Select(Controller);

    string Controller(ControllerModel controller) => $$"""
        namespace RestApi.Generated.Controllers;

        [ApiController]
        public class {{controller.Name}}
        {
            {{ForEach(controller.Actions, Action)}}
        }
    """;

    string Action(ActionModel action) => $$"""
        {{If(action.HasRequestBody, () => $$"""
        public record {{action.Name}}Request(
            {{ForEach(action.BodyParameters, parameter => $"{parameter.Type} @{parameter.Name}", separator: ", ")}}
        );
        """)}}

        [Http{{Method(action.Method)}}]
        [Route("{{action.Route}}")]
        public {{ReturnType(action.Return)}} {{action.Name}}(
            {{ForEach(action.NonBodyParameters, Parameter, separator: ", ")}}
            {{If(action.HasRequestBody, () => $$"""
            , [FromBody] {{action.Name}}Request request
            """)}}
        )
        {
            var __target = {{action.FindTargetStatement}};

            {{Return(action.Return)}} __target.{{action.InvokedMethodName}}(
                {{ForEach(action.InvokedMethodParameters, p => $"@{p.InternalName}: {ParameterLookup(p)}", separator: ", ")}}
            );
        }
    """;

    string Method(HttpMethod method) =>
        $"{method.Method[0]}{method.Method[1..].ToLowerInvariant()}";

    string ReturnType(ReturnModel @return) =>
        @return.Async ? $"async {@return.Type}" :
        @return.Void ? "void" :
        @return.Type
    ;

    string Parameter(ParameterModel parameter) =>
        $"[From{parameter.From}] {parameter.Type} @{parameter.Name}";

    string ParameterLookup(ParameterModel parameter) =>
        $"({parameter.RenderLookup(parameter.FromBody ? $"request.@{parameter.Name}" : $"@{parameter.Name}")})";

    string Return(ReturnModel @return) =>
        @return.Async && @return.Void ? "await" :
        @return.Async && !@return.Void ? "return await" :
        @return.Void ? string.Empty :
        "return";
}
