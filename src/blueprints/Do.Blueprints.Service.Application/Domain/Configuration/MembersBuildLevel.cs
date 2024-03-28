using Do.Domain.Model;
using System.Reflection;

namespace Do.Domain.Configuration;

public class MembersBuildLevel : BuildLevel
{
    internal override void Set(TypeModel typeModel, Type type, DomainModelBuilder builder)
    {
        typeModel.SetMembers(
            BuildConstructorGroup(),
            type.GetProperties(builder.Options.BindingFlags.Property).Select(BuildProperty).ToModelCollection(),
            BuildMethodGroups()
        );

        ConstructorGroupModel? BuildConstructorGroup()
        {
            var constructorInfos = type.GetConstructors(builder.Options.BindingFlags.Constructor) ?? [];
            if (!constructorInfos.Any()) { return null; }

            return new([.. constructorInfos.Select(BuildConstructor)]);
        }

        ConstructorModel BuildConstructor(ConstructorInfo constructorInfo)
        {
            return new(
                "ctor",
                constructorInfo.IsPublic,
                constructorInfo.IsFamily,
                new(constructorInfo.GetCustomAttributes()),
                BuildParameters(constructorInfo)
            );
        }

        PropertyModel BuildProperty(PropertyInfo property) =>
            new(
                property.Name,
                builder.Get(property.PropertyType),
                property.GetMethod?.IsPublic == true,
                property.GetMethod?.IsVirtual == true,
                new(property.GetCustomAttributes())
            );

        ModelCollection<MethodGroupModel> BuildMethodGroups()
        {
            var methods = new Dictionary<string, MethodGroupModel>();
            var methodInfos = type.GetMethods(builder.Options.BindingFlags.Method) ?? [];
            foreach (var group in methodInfos.GroupBy(m => m.Name))
            {
                methods[group.Key] = new(group.Key, [.. group.Select(BuildMethod)]);
            }

            return new(methods.Values);
        }

        MethodModel BuildMethod(MethodInfo methodInfo)
        {
            return new(
                methodInfo.Name,
                methodInfo.IsPublic,
                methodInfo.IsFamily,
                methodInfo.IsVirtual,
                builder.Get(methodInfo.ReturnType),
                new(methodInfo.GetCustomAttributes()),
                BuildParameters(methodInfo)
            );
        }

        ModelCollection<ParameterModel> BuildParameters(MethodBase method) =>
            method.GetParameters().Select(BuildParameter).ToModelCollection();

        ParameterModel BuildParameter(ParameterInfo parameter) =>
            new(
                parameter.Name ?? string.Empty,
                builder.Get(parameter.ParameterType),
                parameter.IsOptional,
                parameter.DefaultValue,
                new(parameter.Member.GetCustomAttributes())
            );
    }
}
