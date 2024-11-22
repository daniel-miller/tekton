using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class JsonWebTokenProvider
    {
        private readonly HttpClient _client;
        private readonly IJsonSerializer _serializer;
        private string _token = string.Empty;
        private string _method = "api/token";
        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        public JsonWebTokenProvider(HttpClient client, IJsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<string> Authenticate(string secret, int? lifetime = null)
        {
            var token = await GetTokenAsync(_client.BaseAddress, secret, lifetime);

            return token;
        }

        public async Task<string> GetTokenAsync(Uri endpoint, string secret, int? lifetime)
        {
            if (ValidateCachedToken())
                return _token;

            return await GenerateNewToken(endpoint, secret, lifetime);
        }

        private async Task<string> GenerateNewToken(Uri endpoint, string secret, int? lifetime)
        {
            string jwt = string.Empty;
            try
            {
                await Lock.WaitAsync();

                var baseUrl = endpoint.ToString();
                if (!baseUrl.EndsWith("/"))
                    baseUrl += "/";

                var request = new JsonWebTokenRequest { Secret = secret, Lifetime = lifetime };
                var requestData = _serializer.Serialize(request);
                var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
                var requestMethod = $"{baseUrl}{_method}";

                var responseMessage = await _client.PostAsync(requestMethod, requestContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                var status = responseMessage.StatusCode;
                if (status != HttpStatusCode.OK)
                {
                    var error = $"{requestMethod} responded with HTTP {(int)status} {status}. {responseContent}";
                    throw new ApiException(error);
                }

                jwt = responseContent;
                _token = jwt;
            }
            finally
            {
                Lock.Release();
            }
            return jwt;
        }

        private bool ValidateCachedToken()
        {
            if (string.IsNullOrEmpty(_token))
                return false;

            var jwt = new JsonWebToken(_serializer, _token);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Key == "exp").Value;
            var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
            if (expiryDateTime > DateTime.UtcNow)
                return true;

            return false;
        }

        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}