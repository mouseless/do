namespace Do.Test;

public class OperationWithGenericParameter<TEntity>
{
    string _parameter = default!;

    internal OperationWithGenericParameter<TEntity> With(string parameter)
    {
        _parameter = parameter;

        return this;
    }

    public string Execute()
    {
        return _parameter;
    }
}
