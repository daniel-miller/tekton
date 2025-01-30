using System;
using System.Net.Http;

namespace Tek.Contract
{
    public interface IHttpClientFactory
    {
        HttpClient Create();

        Uri GetBaseAddress();

        string GetSecret();

        string GetAuthorizationParameter();

        void SetAuthenticationHeader(string scheme, string parameter);
    }
}