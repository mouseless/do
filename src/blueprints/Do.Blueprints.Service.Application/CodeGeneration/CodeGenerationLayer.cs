using Do.Architecture;
using Do.Domain.Model;

using static Do.CodeGeneration.CodeGenerationLayer;

namespace Do.CodeGeneration;

public class CodeGenerationLayer : LayerBase<GenerateCode>
{
    readonly IGeneratedAssemblyCollection _generatedAssemblies = new GeneratedAssemblyCollection();

    protected override PhaseContext GetContext(GenerateCode phase) =>
        phase.CreateContext(_generatedAssemblies);

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new GenerateCode(_generatedAssemblies);
        yield return new Compile();
    }

    public class GenerateCode(IGeneratedAssemblyCollection _generatedAssemblies)
        : PhaseBase<DomainModel>(PhaseOrder.Earliest)
    {
        protected override void Initialize(DomainModel _)
        {
            Context.Add(_generatedAssemblies);
        }
    }

    public class Compile()
        : PhaseBase<IGeneratedAssemblyCollection>(PhaseOrder.Latest)
    {
        protected override void Initialize(IGeneratedAssemblyCollection generatedAssemblies)
        {
            var provider = new GeneratedAssemblyProvider();
            foreach (var descriptor in generatedAssemblies)
            {
                var assembly = new Compiler(descriptor).Compile();

                provider.Add(descriptor.Name, assembly);
            }

            Context.Add(provider);
        }
    }
}
