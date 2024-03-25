﻿using Do.Domain.Model;

namespace Do.Domain.Configuration;

internal class ModelIndexer(ModelIndexerProcessor _indexers) : IDomainService
{
    static IDomainService IDomainService.New(DomainServiceProvider sp) =>
        new ModelIndexer(sp.Get<ModelIndexerProcessor>());

    internal void Execute(DomainModel model)
    {
        _indexers.Apply(model.Types);

        foreach (var methods in model.Types.Select(t => t.Methods))
        {
            _indexers.Apply(methods);

            foreach (var overloads in methods.Select(m => m.Overloads))
            {
                foreach (var overload in overloads)
                {
                    _indexers.Apply(overload.Parameters);
                }
            }
        }

        foreach (var properties in model.Types.Select(t => t.Properties))
        {
            _indexers.Apply(properties);
        }
    }
}