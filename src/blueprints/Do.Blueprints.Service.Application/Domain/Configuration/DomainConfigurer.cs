using Do.Domain.Model;

namespace Do.Domain.Configuration;

internal class DomainConfigurer(IDomainConfiguration _configuration)
{
    internal void Execute(DomainModel model)
    {
        // temporary fix for losing modelindex refernce when
        // using ToModelCollection() extensions
        var types = model.Types.CreateReferenceModelCollection(t => t.IsBuilt(BuildLevel.Members));

        _configuration.Type.Apply(types);

        foreach (var properties in types.Select(t => t.Properties))
        {
            _configuration.Property.Apply(properties);
        }

        foreach (var methods in types.Select(t => t.MethodGroups))
        {
            _configuration.MethodGroup.Apply(methods);
        }

        foreach (var methods in types.Select(t => t.MethodGroups))
        {
            foreach (var overloads in methods.Select(m => m.Methods))
            {
                foreach (var overload in overloads)
                {
                    _configuration.Parameter.Apply(overload.Parameters);
                }
            }
        }
    }
}
