namespace Common
{
    public class JsonWebTokenServerOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public string Whitelist { get; set; }

        public int Lifetime { get; set; }
    }
}