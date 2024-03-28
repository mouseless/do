namespace Do.Architecture;

public class OverlappingPhaseException(PhaseOrder _order, IEnumerable<IPhase> _phases)
    : Exception(
        $"More than one phase cannot have '{_order}' at the same time. " +
        "Change the order of phases. Overlapping phases are: " +
        $"{string.Join(", ", _phases.Where(p => p.Order == _order).Select(p => p.GetType().Name))}"
    )
{ }
