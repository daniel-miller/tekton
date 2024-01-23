namespace Common.Sdk
{
    public class SdkConfiguration
    {
        public string ApiUrl { get; set; }
        public string DeveloperSecret { get; set; }

        public SdkConfiguration(string apiUrl, string developerSecret)
        {
            ApiUrl = apiUrl;
            DeveloperSecret = developerSecret;
        }
    }
}