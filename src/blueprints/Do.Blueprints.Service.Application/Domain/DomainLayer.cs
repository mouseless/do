using Do.Architecture;
using Do.Domain.Configuration;
using Do.Domain.Model;
using Microsoft.Extensions.Configuration;
using static Do.Configuration.ConfigurationLayer;

namespace Do.Domain;

public class DomainLayer : LayerBase<BuildConfiguration>
{
    readonly IDomainTypeCollection _domainTypes = new DomainTypeCollection();
    readonly DomainModelBuilderOptions _domainBuilderOptions = new();
    readonly DomainConventionCollection _domainConventions = new();
    readonly DomainIndexerCollection _domainIndexers = [];

    protected override PhaseContext GetContext(BuildConfiguration phase) =>
        phase.CreateContextBuilder()
            .Add<IDomainTypeCollection>(_domainTypes)
            .Add<DomainModelBuilderOptions>(_domainBuilderOptions)
            .Add<DomainConventionCollection>(_domainConventions)
            .Add<DomainIndexerCollection>(_domainIndexers)
            .Build();

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new BuildDomain(_domainTypes, _domainBuilderOptions, _domainConventions, _domainIndexers);
    }

    public class BuildDomain(
        IDomainTypeCollection _domainTypes,
        DomainModelBuilderOptions _domainBuilderOptions,
        DomainConventionCollection _domainConventions,
        DomainIndexerCollection _domainIndexers
    ) : PhaseBase<ConfigurationManager>(PhaseOrder.Early)
    {
        protected override void Initialize(ConfigurationManager _)
        {
            var builder = new DomainModelBuilder(_domainBuilderOptions);
            var configurer = new DomainConfigurer(new ConventionConfiguration(_domainConventions));
            var indexer = new DomainConfigurer(new IndexerConfiguration(_domainIndexers));

            var model = builder.BuildFrom(_domainTypes);
            configurer.Execute(model);
            indexer.Execute(model);

            Context.Add<DomainModel>(model);
        }
    }
}
