using System.Reflection;

namespace Do.Domain;

public class DomainTypeCollection : List<Type>, IDomainTypeCollection
{
    public void AddFromAssembly(Assembly assembly, Func<Type, bool> except) =>
        AddRange(assembly.GetExportedTypes().Where(t => !except(t)));
}
