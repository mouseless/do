namespace Do.Domain.Model;

public interface IMemberModel : IModel
{
    AttributeCollection CustomAttributes { get; }
}
