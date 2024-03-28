namespace Do.Architecture;

public class CannotProceedException(IEnumerable<IPhase> _phases)
    : Exception(
        "Cannot proceed to run the application. " +
        $"Phases ({string.Join(", ", _phases.Select(p => p.GetType().Name))}) " +
        "won't get ready for initialization."
    )
{ }
