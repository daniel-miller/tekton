namespace Tek.Contract
{
    public class MonitoringSettings
    {
        public bool Debug { get; set; }
        public bool Disabled { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public double? Rate { get; set; }
    }
}
