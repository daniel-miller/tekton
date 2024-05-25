namespace Common
{
    public class ApiTokenRequest
    {
        public string Secret { get; set; }
        public int? Lifetime { get; set; }

        public ApiTokenRequest(string secret, int? lifetime)
        {
            Secret = secret;
            Lifetime = lifetime;
        }
    }
}