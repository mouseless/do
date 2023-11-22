using NHibernate.Exceptions;

namespace Do.ExceptionHandling.Default;

public class GenericADOExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception ex) => ex is GenericADOException;
    public ExceptionInfo Handle(Exception ex) =>
        new(
            (int)((HandledException)ex).StatusCode,
            ex.InnerException != null ? ex.InnerException.Message : ex.Message
        );
}
