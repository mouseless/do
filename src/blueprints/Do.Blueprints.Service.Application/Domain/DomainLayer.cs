using Do.Architecture;
using Do.Domain.Model;
using Microsoft.Extensions.Configuration;

using static Do.Configuration.ConfigurationLayer;

namespace Do.Domain;

public class DomainLayer : LayerBase<BuildConfiguration>
{
    readonly IDomainAssemblyCollection _domainAssemblies = new DomainAssemblyCollection();
    readonly IDomainTypeCollection _domainTypes = new DomainTypeCollection();
    readonly DomainBuilderOptions _domainBuilderOptions = new();

    protected override PhaseContext GetContext(BuildConfiguration phase) =>
        phase.CreateContextBuilder()
            .Add<IDomainAssemblyCollection>(_domainAssemblies)
            .Add<IDomainTypeCollection>(_domainTypes)
            .Add<DomainBuilderOptions>(_domainBuilderOptions)
            .Build();

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new BuildDomain(_domainAssemblies, _domainTypes, _domainBuilderOptions);
    }

    public class BuildDomain(IDomainAssemblyCollection _domainAssemblies, IDomainTypeCollection _domainTypes, DomainBuilderOptions _domainBuilderOptions)
        : PhaseBase<ConfigurationManager>(PhaseOrder.Early)
    {
        protected override void Initialize(ConfigurationManager _)
        {
            var builder = new DomainModelBuilder(_domainBuilderOptions);
            var model = builder.BuildFrom(_domainAssemblies, _domainTypes);

            Context.Add<DomainModel>(model);
        }
    }
}
