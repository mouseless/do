namespace Do.Architecture;

public class LayerConfigurator
{
    public static LayerConfigurator Create<TTarget>(ApplicationContext context, TTarget target)
        where TTarget : notnull
    {
        return new(context, new Target(typeof(TTarget), target));
    }

    public static LayerConfigurator Create<TTarget1, TTarget2>(ApplicationContext context, TTarget1 target1, TTarget2 target2)
        where TTarget1 : notnull
        where TTarget2 : notnull
    {
        return new(
            context,
            new Target(typeof(TTarget1), target1),
            new Target(typeof(TTarget2), target2)
        );
    }

    public static LayerConfigurator Create<TTarget1, TTarget2, TTarget3>(ApplicationContext context, TTarget1 target1, TTarget2 target2, TTarget3 target3)
        where TTarget1 : notnull
        where TTarget2 : notnull
        where TTarget3 : notnull
    {
        return new(
            context,
            new Target(typeof(TTarget1), target1),
            new Target(typeof(TTarget2), target2),
            new Target(typeof(TTarget3), target3)
        );
    }

    record Target(Type Type, object Value);

    readonly ApplicationContext _context;
    readonly List<Target> _targets;

    LayerConfigurator(ApplicationContext context, params Target[] targets)
    {
        _context = context;
        _targets = [.. targets];
    }

    public ApplicationContext Context => _context;

    public void Configure<TTarget>(Action<TTarget> configuration)
    {
        if (!Matches(typeof(TTarget))) { return; }

        configuration(ValueAs<TTarget>(0));
    }

    public void Configure<TTarget1, TTarget2>(Action<TTarget1, TTarget2> configuration)
    {
        if (!Matches(typeof(TTarget1), typeof(TTarget2))) { return; }

        configuration(ValueAs<TTarget1>(0), ValueAs<TTarget2>(1));
    }

    public void Configure<TTarget1, TTarget2, TTarget3>(Action<TTarget1, TTarget2, TTarget3> configuration)
    {
        if (!Matches(typeof(TTarget1), typeof(TTarget2), typeof(TTarget3))) { return; }

        configuration(ValueAs<TTarget1>(0), ValueAs<TTarget2>(1), ValueAs<TTarget3>(2));
    }

    bool Matches(params Type[] types)
    {
        if (_targets.Count != types.Length) { return false; }

        for (var i = 0; i < _targets.Count; i++)
        {
            if (_targets[i].Type != types[i]) { return false; }
        }

        return true;
    }

    TTarget ValueAs<TTarget>(int index) => (TTarget)_targets[index].Value;

    public override bool Equals(object? obj)
    {
        if (obj is not LayerConfigurator configurator) return false;
        if (_targets.Count != configurator._targets.Count) return false;

        for (var i = 0; i < _targets.Count; i++)
        {
            if (_targets[i].Type != configurator._targets[i].Type) { return false; }
            if (!Equals(_targets[i].Value, configurator._targets[i].Value)) { return false; }
        }

        return true;
    }

    public override int GetHashCode() => HashCode.Combine(_targets);
}
