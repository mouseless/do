namespace Do.Domain.Configuration;

public class TypeBuildContext
{
    readonly Type _type;
    readonly DomainModelBuilder _builder;

    internal TypeBuildContext(Type type, DomainModelBuilder builder)
    {
        _type = type;
        _builder = builder;
    }

    public Type Type => _type;

    public bool IsDomainType(Type type) =>
        _builder.IsDomainType(type);
}
