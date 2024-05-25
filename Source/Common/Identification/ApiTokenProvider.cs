using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class ApiTokenProvider
    {
        private readonly HttpClient _httpClient;
        private string _cachedToken = string.Empty;
        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        public ApiTokenProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync(Uri endpoint, string secret, int? lifetime)
        {
            if (ValidateCachedToken())
                return _cachedToken;

            return await GenerateNewToken(endpoint, secret, lifetime);
        }

        private async Task<string> GenerateNewToken(Uri endpoint, string secret, int? lifetime)
        {
            string newToken = string.Empty;
            try
            {
                await Lock.WaitAsync();

                var request = new ApiTokenRequest(secret, lifetime);
                var requestData = JsonSerializer.Serialize(request);
                var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
                var requestMethod = $"{endpoint}api/token";

                var responseMessage = await _httpClient.PostAsync(requestMethod, requestContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                
                var status = responseMessage.StatusCode;
                if (status != HttpStatusCode.OK)
                {
                    var error = $"{requestMethod} responded with HTTP {(int)status} {status}. {responseContent}";
                    throw new ApiException(error);
                }

                newToken = responseContent;
                _cachedToken = newToken;
            }
            finally
            {
                Lock.Release();
            }
            return newToken;
        }

        private bool ValidateCachedToken()
        {
            if (string.IsNullOrEmpty(_cachedToken))
                return false;
            
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
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