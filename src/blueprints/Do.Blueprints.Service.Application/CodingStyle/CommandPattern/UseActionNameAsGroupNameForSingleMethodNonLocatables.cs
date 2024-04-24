﻿using Do.Business;
using Do.RestApi.Configuration;

namespace Do.CodingStyle.CommandPattern;

public class UseActionNameAsGroupNameForSingleMethodNonLocatables : IApiModelConvention<ControllerModelContext>
{
    public void Apply(ControllerModelContext context)
    {
        if (!context.Controller.TypeModel.TryGetMetadata(out var metadata)) { return; }
        if (metadata.Has<LocatableAttribute>()) { return; }
        if (context.Controller.Action.Count != 1) { return; }

        var theOnlyAction = context.Controller.Actions.Single();
        context.Controller.GroupName = theOnlyAction.Name;
    }
}