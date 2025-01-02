using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Atomic.Common
{
    public interface IHttpClientFactory
    {
        HttpClient Create();

        Uri GetBaseAddress();

        string GetSecret();

        string GetToken();

        void SetToken(string token);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private Uri _baseAddress;
        private string _secret;
        private string _token;

        public HttpClientFactory(Uri baseAddress,  string secret)
        {
            _baseAddress = baseAddress;
            _secret = secret;
        }

        public HttpClient Create()
        {
            var client = new HttpClient() 
            { 
                BaseAddress = _baseAddress
            };

            if (!string.IsNullOrWhiteSpace(_token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            return client;
        }

        public Uri GetBaseAddress()
        {
            return _baseAddress;
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