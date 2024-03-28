using System.Reflection;

namespace Do.Domain;

public interface IDomainTypeCollection : ICollection<Type>, IEnumerable<Type>, IEnumerable, IList<Type>
{
    void AddFromAssembly(Assembly assembly, Func<Type, bool> except);
}
