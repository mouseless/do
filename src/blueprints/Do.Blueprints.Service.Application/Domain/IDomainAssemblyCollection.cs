using System.Reflection;

namespace Do.Domain;

public interface IDomainAssemblyCollection : ICollection<Assembly>, IEnumerable<Assembly>, IEnumerable, IList<Assembly> { }
