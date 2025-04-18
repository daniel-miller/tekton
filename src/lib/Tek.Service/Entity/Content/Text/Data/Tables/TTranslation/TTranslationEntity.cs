namespace Tek.Service.Content;

public partial class TTranslationEntity
{
    public Guid TranslationId { get; set; }

    public string TranslationText { get; set; } = null!;

    public DateTime ModifiedWhen { get; set; }
}