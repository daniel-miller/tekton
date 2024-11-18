using System.Net.Http;
using System.Net.Http.Headers;

namespace Common
{
    public interface IHttpClientCreator
    {
        HttpClient Create();

        string GetSecret();

        string GetToken();

        void SetToken(string token);
    }

    public class HttpClientCreator : IHttpClientCreator
    {
        private string _token;
        private string _secret;

        public HttpClientCreator(string secret)
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

        public void SetToken(string jwt)
        {
            _token = jwt;
        }
    }
}