namespace Tek.Contract
{
    public class ApiSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string CallbackUrl { get; set; }
        public CertificateSettings Certificate { get; set; }
    }
}