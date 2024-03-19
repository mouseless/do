using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Do.CodeGeneration;

public record GeneratedAssemblyDescriptor(string Name)
{
    public List<string> Codes { get; } = new();
    public CSharpCompilationOptions CompilationOptions { get; set; } = new(OutputKind.DynamicallyLinkedLibrary);
    public List<Assembly> References { get; } = new();
}
