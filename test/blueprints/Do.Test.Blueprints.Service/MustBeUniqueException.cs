using Do.ExceptionHandling;

namespace Do.Test;

public class MustBeUniqueException(string propertyName)
    : HandledException($"{propertyName} should be unique")
{ }
