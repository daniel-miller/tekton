namespace Tek.Contract
{
    public class PluginSettings
    {
        public IntegrationSettings Integration { get; set; }
    }

    public class IntegrationSettings
    {
        public AstronomyApiSettings AstronomyApi { get; set; }
        public VisualCrossingSettings VisualCrossing { get; set; }
    }

    public class AstronomyApiSettings
    {
        public string Host { get; set; }
        public string App { get; set; }
        public string Secret { get; set; }
    }

    public class VisualCrossingSettings
    {
        public string Host { get; set; }
        public string Secret { get; set; }
    }
}
