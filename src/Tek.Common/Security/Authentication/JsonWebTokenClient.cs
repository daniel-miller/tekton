using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tek.Common
{
    public class JsonWebTokenClient
    {
        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IJsonSerializer _jsonSerializer;

        private string _token = null;

        public JsonWebTokenClient(IHttpClientFactory httpClientFactory, IJsonSerializer serializer)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializer = serializer;
        }

        public async Task<string> NewTokenAsync(Uri jwtServer, JsonWebTokenRequest jwtRequest)
        {
            _token = null;

            return await GetTokenAsync(jwtServer, jwtRequest);
        }

        public async Task<string> GetTokenAsync(Uri jwtServer, JsonWebTokenRequest jwtRequest)
        {
            if (ValidateCachedToken())
                return _token;

            return await GenerateNewToken(jwtServer, jwtRequest);
        }

        private async Task<string> GenerateNewToken(Uri jwtServer, JsonWebTokenRequest jwtRequest)
        {
            string jwt = string.Empty;
            try
            {
                await Lock.WaitAsync();

                if (jwtRequest.Lifetime == null || jwtRequest.Lifetime <= 0)
                    jwtRequest.Lifetime = JsonWebToken.DefaultLifetimeLimit;

                var jwtServerUrl = jwtServer.AbsoluteUri;

                var requestData = _jsonSerializer.Serialize(jwtRequest);
                var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

                using (var httpClient = _httpClientFactory.Create())
                {
                    var responseMessage = await httpClient.PostAsync(jwtServerUrl, requestContent);
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();

                    var status = responseMessage.StatusCode;
                    if (status != HttpStatusCode.OK)
                    {
                        var error = $"{jwtServerUrl} responded with HTTP {(int)status} {status}. {responseContent}";
                        throw new ApiException(error);
                    }

                    jwt = responseContent;
                    _token = jwt;
                }
            }
            finally
            {
                Lock.Release();
            }
            return jwt;
        }

        /// <remarks>
        /// Client-side validation of a JWT is simple: ensure the JWT can be deserialized into a valid object, and
        /// confirm it is not yet expired. All other validation (e.g., signature verification) can be done only on the
        /// server-side.
        /// </remarks>
        private bool ValidateCachedToken()
        {
            if (string.IsNullOrWhiteSpace(_token))
                return false;

            try
            {
                var jwt = new JsonWebToken(_jsonSerializer, _token);

                return !jwt.IsExpired();
            }
            catch
            {
                return false;
            }
        }
    }
}