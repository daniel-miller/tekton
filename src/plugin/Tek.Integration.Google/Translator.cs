using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;

using Tek.Base;

namespace Tek.Integration.Google
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string fromText, string fromLanguage, string toLanguage);
    }

    public class Translator : ITranslator
    {
        private readonly ObsoleteTranslationService _service;
        private readonly HashSet<string> _languages;
        private readonly GoogleServiceAccount _account;
        private readonly string _accountJson;

        public Translator(ObsoleteTranslationService service, GoogleTranslationSettings settings)
        {
            _service = service;
            _languages = settings.Languages.ToHashSet(StringComparer.OrdinalIgnoreCase);
            _account = settings.Account;
            _accountJson = SnakeCaseSerializer.Serialize<GoogleServiceAccount>(_account);
        }

        public bool IsLanguageSupported(string language) 
            => _languages.Contains(language);

        public async Task<string> TranslateAsync(string fromText, string fromLanguage, string toLanguage)
        {
            if (!IsLanguageSupported(toLanguage))
                throw new UnsupportedLanguageException(toLanguage);

            var previous = await _service.GetAsync(fromText, fromLanguage, toLanguage);
            if (previous != null)
                return previous;

            var client = await new TranslationServiceClientBuilder { JsonCredentials = _accountJson }.BuildAsync();
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = fromLanguage,
                Contents = { fromText },
                MimeType = "text/plain",
                TargetLanguageCode = toLanguage,
                Parent = new ProjectName(_account.ProjectId).ToString(),
            };

            var response = await client.TranslateTextAsync(request);
            var translation = response.Translations[0];
            var toText = translation.TranslatedText;

            if (await _service.ExistsAsync(fromText, fromLanguage))
                await _service.UpdateAsync(fromLanguage, fromText, toLanguage, toText);
            else
                await _service.InsertAsync(fromLanguage, fromText, toLanguage, toText);

            return toText;
        }
    }
}