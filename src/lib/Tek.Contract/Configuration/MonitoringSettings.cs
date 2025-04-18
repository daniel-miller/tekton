namespace Tek.Contract
{
    public class MonitoringSettings
    {
        public bool Debug { get; set; }
        public bool Disabled { get; set; }
        public string File { get; set; }
        public string Url { get; set; }
        public double? Rate { get; set; }

        public bool Enabled => !Disabled;
    }
}
