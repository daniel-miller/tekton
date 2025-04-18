using System;

namespace Tek.Contract
{
    public class CreateTranslation
    {
        public Guid TranslationId { get; set; }

        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class ModifyTranslation
    {
        public Guid TranslationId { get; set; }

        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class DeleteTranslation
    {
        public Guid TranslationId { get; set; }
    }
}