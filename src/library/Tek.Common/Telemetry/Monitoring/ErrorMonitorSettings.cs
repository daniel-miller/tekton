namespace Tek.Common
{
    public class ErrorMonitorSettings
    {
        public bool Debug { get; set; }
        public bool Enabled { get; set; }
        
        public string Url { get; set; }
        public string Log { get; set; }
        
        public double Rate { get; set; }

        public bool Valid => !string.IsNullOrWhiteSpace(Url) && Rate > 0;
    }
}