using Do.Business;
using Do.Business.Default;
using Do.Domain.Model;
using System.Reflection;

namespace Do;

public static class DefaultBusinessExtensions
{
    public static DefaultBusinessFeature Default(this BusinessConfigurator _, List<Assembly> assemblies) =>
        new(assemblies);

    internal static bool IsIgnored(this TypeModel type) =>
        !type.IsBusinessType ||
        !type.IsPublic ||
        type.IsInterface ||
        type.Namespace?.StartsWith("System") == true ||
        (type.IsSealed && type.IsAbstract) || // if type is static
        type.IsAbstract ||
        type.IsValueType ||
        type.IsGenericMethodParameter ||
        type.IsGenericTypeParameter ||
        type.IsAssignableTo<MulticastDelegate>() ||
        type.IsAssignableTo<Exception>() ||
        type.IsAssignableTo<Attribute>() ||
        (type.ContainsGenericParameters && !type.GenericTypeArguments.Any()) ||
        type.Methods.Contains("<Clone>$") // if type is record
    ;

    internal static bool IsTransient(this TypeModel type) =>
        type.Methods.TryGetValue("With", out var with) && with.CanReturn(type);

    internal static bool IsScoped(this TypeModel type) =>
        type.IsAssignableTo<IScoped>();

    internal static bool IsSingleton(this TypeModel type) =>
        !type.IsTransient() && !type.IsScoped() && type.Properties.All(p => !p.IsPublic);

    internal static bool IsPrimitive(this TypeModel type) =>
        type.IsValueType || type.IsAssignableTo<string>();

    internal static bool IsPrimitiveList(this TypeModel type) =>
        type.IsGenericType &&
        type.GenericTypeDefinition?.IsAssignableTo(typeof(List<>)) == true &&
        type.GenericTypeArguments.First().IsPrimitive();

    internal static bool IsEntity(this TypeModel type) =>
        type.IsClass && !type.IsAbstract &&
        type.Constructor?.Overloads.Any(c => c
            .Parameters.Any(p => p
                .ParameterType.Name.StartsWith("IEntityContext")
            )
        ) == true;
}
