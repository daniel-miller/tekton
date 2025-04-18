using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertTranslation : Query<bool>
    {
        public Guid TranslationId { get; set; }
    }

    public class FetchTranslation : Query<TranslationModel>
    {
        public Guid TranslationId { get; set; }
    }

    public class CollectTranslations : Query<IEnumerable<TranslationModel>>, ITranslationCriteria
    {
        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class CountTranslations : Query<int>, ITranslationCriteria
    {
        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class SearchTranslations : Query<IEnumerable<TranslationMatch>>, ITranslationCriteria
    {
        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public interface ITranslationCriteria
    {
        Filter Filter { get; set; }
        
        string TranslationText { get; set; }

        DateTime ModifiedWhen { get; set; }
    }

    public partial class TranslationMatch
    {
        public Guid TranslationId { get; set; }
    }

    public partial class TranslationModel
    {
        public Guid TranslationId { get; set; }

        public string TranslationText { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }
}