namespace Do.RestApi.Configuration;

public interface IApiModelConvention { }

public interface IApiModelConvention<TContext> : IApiModelConvention
{
    void Apply(TContext context);
}
