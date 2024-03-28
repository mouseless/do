using Do.ExceptionHandling;

namespace Do.Test;

public class TestServiceHandledException(string message)
    : HandledException(message)
{ }
