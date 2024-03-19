namespace Do.Test;

public class TransientWithTask
{
    internal Task<TransientWithTask> With() => Task.FromResult(this);
}
