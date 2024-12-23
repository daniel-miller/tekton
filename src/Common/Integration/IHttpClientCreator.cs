using System.Net.Http;
using System.Net.Http.Headers;

namespace Common
{
    public interface IHttpClientFactory
    {
        HttpClient Create();

        string GetSecret();

        string GetToken();

        void SetToken(string token);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private string _token;
        private string _secret;
        
        public HttpClientFactory(string secret)
        {
            _secret = secret;
        }

        public HttpClient Create()
        {
            var client = new HttpClient();

            if (!string.IsNullOrEmpty(_token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            return client;
        }

        public string GetSecret()
        {
            return _secret;
        }

        public string GetToken()
        {
            return _token;
        }

        public void SetToken(string token)
        {
            _token = token;
        }
    }
}