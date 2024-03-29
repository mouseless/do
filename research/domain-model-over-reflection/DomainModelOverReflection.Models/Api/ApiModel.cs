﻿using DomainModelOverReflection.Models.Domain;
using System.Reflection;

namespace DomainModelOverReflection.Api;

public class ApiModel
{
    public static ApiModel Build(Assembly? assembly)
    {
        var model = new ApiModel();
        var types = assembly?.GetTypes() ?? Array.Empty<Type>();
        foreach (var type in types)
        {
            if (type.Namespace is not null && type.Namespace.EndsWith("Business"))
            {
                if (type.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Any()))
                {
                    model.ControllerModels.Add(new(type));
                }
                else
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    if (methods.Any(m => m.Name == "With") && methods.Any(m => m.Name == "Process"))
                    {
                        model.ControllerModels.Add(new(type));
                    }
                }
            }
        }

        return model;
    }

    public static ApiModel Build(IDomainModel domainModel)
    {
        var model = new ApiModel();
        foreach (var type in domainModel.TypeModels)
        {
            if (type.Constructors?.Any(c => c.IsPublic && c.Parameters?.Any() == true) == true)
            {
                model.ControllerModels.Add(new(type));
            }
            else
            {
                if (type.Methods?.Any(m => m.Name == "With") == true && type.Methods.Any(m => m.Name == "Process"))
                {
                    model.ControllerModels.Add(new(type));
                }
            }
        }

        return model;
    }

    public List<ControllerModel> ControllerModels { get; } = new();
}
