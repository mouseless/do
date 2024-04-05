﻿using Do.Architecture;
using Do.Authentication;
using Do.Authentication.FixedToken;
using Do.RestApi.Model;
using Microsoft.OpenApi.Models;

namespace Do.Test.ConfigurationOverrider;

public class ConfigurationOverriderFeature : IFeature
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureAutoPersistenceModel(model =>
        {
            model.Override<Entity>(x => x.Map(e => e.String).Length(500));
            model.Override<Entity>(x => x.Map(e => e.Unique).Column("UniqueString").Unique());
        });

        configurator.ConfigureApiModel(apiModel =>
        {
            var domainModel = configurator.Context.GetDomainModel();

            apiModel.References.Add<Middleware>();

            apiModel.Controller[nameof(Singleton)].Action[nameof(Singleton.GetTime)].AdditionalAttributes.Add(typeof(UseAttribute<Middleware>).GetCSharpFriendlyFullName());
            apiModel.Controller[nameof(AuthenticationTests)].Action[nameof(AuthenticationTests.TestFormPostAuthentication)].AdditionalAttributes.Add(typeof(UseAttribute<Middleware>).GetCSharpFriendlyFullName());
            apiModel.Controller[nameof(ExceptionResult)].Action[nameof(ExceptionResult.Throw)].Parameter["handled"].From = ParameterModelFrom.Query;
            apiModel.Controller[nameof(AuthenticationTests)].Action[nameof(AuthenticationTests.TestFormPostAuthentication)].UseForm = true;

            apiModel.Controller[nameof(Entities)].AddSingleById<Entity>(domainModel);
        });

        configurator.ConfigureSwaggerGenOptions(swaggerGenOptions =>
        {
            swaggerGenOptions.AddSecurityDefinition("AdditionalSecurity",
                new()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-Secret",
                    Description = "Enter secret information",
                }
            );

            swaggerGenOptions.AddSecurityRequirementToOperationsThatUse<Middleware>("AdditionalSecurity");
            swaggerGenOptions.AddParameterToOperationsThatUse<Middleware>("X-Security", @in: ParameterLocation.Header, required: true);
        });

    }
}
