namespace Do.Orm;

[AttributeUsage(AttributeTargets.Class)]
public class QueryAttribute(Type type) : Attribute
{
    public Type Type { get; } = type;
}
