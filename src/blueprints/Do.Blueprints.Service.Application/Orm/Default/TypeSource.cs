using FluentNHibernate;
using FluentNHibernate.Diagnostics;

namespace Do.Orm.Default;

internal class TypeSource : ITypeSource
{
    readonly List<Type> _types = [];

    public void Add(Type type) => _types.Add(type);

    string ITypeSource.GetIdentifier() =>
        nameof(TypeSource);
    IEnumerable<Type> ITypeSource.GetTypes() =>
        _types;
    void ITypeSource.LogSource(IDiagnosticLogger logger) { }
}
