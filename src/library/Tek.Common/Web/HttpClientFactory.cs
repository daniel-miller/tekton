using System;
using System.Net.Http;
using System.Net.Http.Headers;

using Tek.Contract;

namespace Tek.Common
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Uri _baseAddress;
        
        private readonly string _secret;

        private string _authorizationScheme = "Bearer";

        private string _authorizationParameter;

        public HttpClientFactory(Uri baseAddress, string secret)
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

            if (_authorizationParameter.IsNotEmpty())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authorizationScheme, _authorizationParameter);
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

        public string GetAuthorizationParameter()
        {
            return _authorizationParameter;
        }

        public void SetAuthenticationHeader(string scheme, string parameter)
        {
            _authorizationScheme = scheme;
            _authorizationParameter = parameter;
        }
    }
}