using Do.Domain.Model;
using Do.Orm;
using Do.RestApi.Configuration;
using Do.RestApi.Model;

using ParameterModel = Do.RestApi.Model.ParameterModel;

namespace Do.Business.Default.RestApiConventions;

public class LookupEntityByIdConvention(DomainModel _domain)
    : IApiModelConvention<ParameterModelContext>
{
    public void Apply(ParameterModelContext context)
    {
        if (!_domain.Types.TryGetValue(context.Parameter.Type, out var entityType)) { return; }
        if (!entityType.IsEntity()) { return; }

        var queryContextType = FindQueryContextType(entityType);

        var queryContextParameter = new ParameterModel(ParameterModelFrom.Services, queryContextType.CSharpFriendlyFullName, $"{entityType.Name}Query");
        context.Action.Parameter[queryContextParameter.Name] = queryContextParameter;

        context.Parameter.Type = nameof(Guid);
        context.Parameter.Name += "Id";
        context.Parameter.RenderLookup = parameterExpression => $"{queryContextParameter.Name}.SingleById({parameterExpression})";
    }

    TypeModel FindQueryContextType(TypeModel entityType)
    {
        TypeModel? queryContextType = default;

        entityType.Apply(t => queryContextType = _domain.Types[typeof(IQueryContext<>).MakeGenericType(t)]);

        return queryContextType ?? throw new("Query type should've existed");
    }
}
