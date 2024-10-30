using System.Net.Http;

namespace Common
{
    public interface IApiClientFactory
    {
        HttpClient CreateClient();

        string GetSecret();

        string GetToken();

        void SetToken(string token);
    }
}