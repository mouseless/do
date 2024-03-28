namespace Do.Domain.Configuration;

public interface IConventionComponent<T>
{
    static abstract T New();
}
