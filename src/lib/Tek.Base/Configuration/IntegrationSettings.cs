using System.Collections.Generic;

namespace Tek.Base
{
    public class IntegrationSettings
    {
        public AstronomyApiSettings Astronomy { get; set; }
        public GoogleApiSettings Google { get; set; }
        public VisualCrossingApiSettings VisualCrossing { get; set; }
    }

    public class AstronomyApiSettings
    {
        public string Host { get; set; }
        public string App { get; set; }
        public string Secret { get; set; }
    }

    public class GoogleApiSettings
    {
        public GoogleTranslationSettings Translation { get; set; }
    }

    public class GoogleTranslationSettings
    {
        public List<string> Languages { get; set; }
        public GoogleServiceAccount Account { get; set; }
    }

    public class GoogleServiceAccount
    {
        public string Type { get; set; }
        public string ProjectId { get; set; }
        public string PrivateKeyId { get; set; }
        public string PrivateKey { get; set; }
        public string ClientEmail { get; set; }
        public string ClientId { get; set; }
        public string AuthUri { get; set; }
        public string TokenUri { get; set; }
        public string AuthProviderX509CertUrl { get; set; }
        public string ClientX509CertUrl { get; set; }
    }

    public class VisualCrossingApiSettings
    {
        public string Host { get; set; }
        public string Secret { get; set; }
    }
}
