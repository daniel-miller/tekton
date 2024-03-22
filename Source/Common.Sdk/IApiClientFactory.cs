using System.Net.Http;

namespace Common.Sdk
{
    public interface IApiClientFactory
    {
        HttpClient CreateClient();

        string GetSecret();

        string GetToken();

        void SetToken(string token);
    }
}