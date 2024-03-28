namespace Do.Architecture;

public class PhaseContext(IEnumerable<LayerConfigurator> configurators)
    : IDisposable
{
    public static readonly PhaseContext Empty = new([]);

    public IEnumerable<LayerConfigurator> Configurators { get; } = [.. configurators];
    public Action? OnDispose { get; init; }

    void IDisposable.Dispose() => OnDispose?.Invoke();
}
