using Do.RestApi.Configuration;
using Do.RestApi.Model;

namespace Do.RestApi;

public interface IApiModelConventionCollection : IList<IApiModelConvention>
{
    public void Apply(ApiModel apiModel)
    {
        var apiModelContext = new ApiModelContext { Api = apiModel };
        foreach (var apiModelConvention in this.OfType<IApiModelConvention<ApiModelContext>>())
        {
            apiModelConvention.Apply(apiModelContext);
        }

        foreach (ControllerModel controller in apiModel.Controllers.ToList())
        {
            var controllerModelContext = new ControllerModelContext { Api = apiModel, Controller = controller };
            foreach (var controllerConvention in this.OfType<IApiModelConvention<ControllerModelContext>>())
            {
                controllerConvention.Apply(controllerModelContext);
            }
        }

        foreach (ControllerModel controller in apiModel.Controllers.ToList())
        {
            foreach (ActionModel action in controller.Actions.ToList())
            {
                var actionModelContext = new ActionModelContext { Api = apiModel, Controller = controller, Action = action };
                foreach (var actionConvention in this.OfType<IApiModelConvention<ActionModelContext>>())
                {
                    actionConvention.Apply(actionModelContext);
                }
            }
        }

        foreach (ControllerModel controller in apiModel.Controllers.ToList())
        {
            foreach (ActionModel action in controller.Actions.ToList())
            {
                foreach (ParameterModel parameter in action.Parameters.ToList())
                {
                    var parameterModelContext = new ParameterModelContext { Api = apiModel, Controller = controller, Action = action, Parameter = parameter };
                    foreach (var parameterConvention in this.OfType<IApiModelConvention<ParameterModelContext>>())
                    {
                        parameterConvention.Apply(parameterModelContext);
                    }
                }
            }
        }
    }
}
