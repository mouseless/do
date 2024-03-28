using System.Reflection;

namespace Do.Domain.Configuration;

public class DomainModelBuilderOptions
{
    public List<TypeBuildLevelFilter> BuildLevels { get; set; } = [];
    public BindingFlagOptions BindingFlags { get; } = new();

    public class BindingFlagOptions
    {
        public BindingFlags Constructor { get; set; } = System.Reflection.BindingFlags.Default;
        public BindingFlags Method { get; set; } = System.Reflection.BindingFlags.Default;
        public BindingFlags Property { get; set; } = System.Reflection.BindingFlags.Default;
    }
}
