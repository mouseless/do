using Do.Architecture;
using Do.CodeGeneration;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Do;

public static class CodeGenerationExtensions
{
    public static void AddCodeGeneration(this ICollection<ILayer> layers) => layers.Add(new CodeGenerationLayer());

    public static IGeneratedAssemblyCollection GetGeneratedAssemblyCollection(this ApplicationContext source) => source.Get<IGeneratedAssemblyCollection>();
    public static GeneratedAssemblyProvider GetGeneratedAssemblyProvider(this ApplicationContext source) => source.Get<GeneratedAssemblyProvider>();
    public static Assembly GetGeneratedAssembly(this ApplicationContext source, string name) => source.GetGeneratedAssemblyProvider()[name];

    public static void ConfigureGeneratedAssemblyCollection(this LayerConfigurator configurator, Action<IGeneratedAssemblyCollection> configuration) => configurator.Configure(configuration);

    public static void Add(this IGeneratedAssemblyCollection generatedAssemblies, string name, Action<GeneratedAssemblyDescriptor> descriptorBuilder,
        Func<CSharpCompilationOptions, CSharpCompilationOptions>? compilationOptionsBuilder = default
    )
    {
        var descriptor = new GeneratedAssemblyDescriptor(name);

        descriptorBuilder(descriptor);
        descriptor.CompilationOptions = compilationOptionsBuilder?.Invoke(descriptor.CompilationOptions) ?? descriptor.CompilationOptions;

        generatedAssemblies.Add(descriptor);
    }

    public static GeneratedAssemblyDescriptor AddCode(this GeneratedAssemblyDescriptor descriptor, string code)
    {
        descriptor.Codes.Add(code);

        return descriptor;
    }

    public static GeneratedAssemblyDescriptor AddReferenceFrom<T>(this GeneratedAssemblyDescriptor descriptor) => descriptor.AddReferenceFrom(typeof(T));
    public static GeneratedAssemblyDescriptor AddReferenceFrom(this GeneratedAssemblyDescriptor descriptor, Type type) => descriptor.AddReference(type.Assembly);
    public static GeneratedAssemblyDescriptor AddReference(this GeneratedAssemblyDescriptor descriptor, Assembly reference)
    {
        descriptor.References.Add(reference);

        return descriptor;
    }
}
