using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace Do.CodeGeneration;

public class Compiler(GeneratedAssemblyDescriptor _descriptor)
{
    readonly Dictionary<string, MetadataReference> _references = new();

    void AddReference(Assembly assembly)
    {
        if (_references.ContainsKey(assembly.Location)) { return; }

        _references.Add(assembly.Location, MetadataReference.CreateFromFile(assembly.Location));
        foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
        {
            AddReference(Assembly.Load(referencedAssembly));
        }
    }

    public Assembly Compile()
    {
        foreach (var assembly in _descriptor.References)
        {
            AddReference(assembly);
        }

        _descriptor.AddCode(string.Join(
            Environment.NewLine,
            _descriptor.CompilationOptions.Usings.Select(u => $"global using global::{u};")
        ));

        var compilation = CSharpCompilation.Create(
            Path.GetRandomFileName(),
            syntaxTrees: _descriptor.Codes.Select(c => CSharpSyntaxTree.ParseText(c)),
            references: _references.Values,
            options: _descriptor.CompilationOptions
        );

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);
        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error
            );
            var errors = new StringBuilder();
            foreach (var diagnostic in failures)
            {
                errors.AppendLine(diagnostic.GetMessage());
                errors.AppendLine();
            }

            throw new Exception($"{errors}");
        }

        ms.Seek(0, SeekOrigin.Begin);

        return Assembly.Load(ms.ToArray());
    }
}
