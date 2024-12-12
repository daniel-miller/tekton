namespace Common
{
    public class SentryOptions
    {
        public string Dsn { get; set; }
        public bool Enabled { get; set; }
        public string Log { get; set; }
        public double Rate { get; set; }

        public bool Disabled => !Enabled || !Valid;
        public bool Valid => !string.IsNullOrEmpty(Dsn) && Rate > 0;
    }
}