using Do.Domain.Model;

namespace Do.Domain.Configuration;

public abstract class BuildLevel
{
    static readonly List<BuildLevel> _all = [];

    public static readonly BuildLevel Basics = new BasicsBuildLevel();
    public static readonly BuildLevel Generics = new GenericsBuildLevel();
    public static readonly BuildLevel Inheritance = new InheritanceBuildLevel();
    public static readonly BuildLevel Metadata = new MetadataBuildLevel();
    public static readonly BuildLevel Members = new MembersBuildLevel();

    public static IReadOnlyCollection<BuildLevel> All => _all.AsReadOnly();

    int _value;

    protected BuildLevel()
    {
        _value = _all.Count;

        _all.Add(this);
    }

    public bool Covers(BuildLevel other) =>
        _value >= other._value;

    internal abstract void Set(TypeModel typeModel, Type type, DomainModelBuilder builder);
}