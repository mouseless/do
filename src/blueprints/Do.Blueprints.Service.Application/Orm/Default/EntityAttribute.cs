namespace Do.Orm;

[AttributeUsage(AttributeTargets.Class)]
public class EntityAttribute(Type type) : Attribute
{
    public Type Type { get; } = type;
}
