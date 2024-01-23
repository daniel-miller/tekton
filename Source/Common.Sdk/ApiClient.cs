using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Common.Contract;

namespace Common.Sdk
{
    public class ApiClient : IDisposable
    {
        private readonly string _apiUrl;
        private HttpClient _client;
        private Pagination _pagination;

        public ApiClient(string apiUrl)
        {
            _apiUrl = apiUrl;
            _client = new HttpClient();
        }

        public ApiClient(string apiUrl, string developerSecret)
        {
            _apiUrl = apiUrl;
            _client = new HttpClient();

            var token = GetApiToken(developerSecret);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetApiToken(string secret)
        {
            var provider = new ApiTokenProvider(_client);
            var token = Task.Run(() => provider.GetTokenAsync(secret, _apiUrl))
                .GetAwaiter().GetResult();
            return token;
        }

        private void SetApiToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public T HttpGet<T>(string endpoint, string id)
        {
            return HttpGet<T>(endpoint, new[] { id });
        }

        public T HttpGet<T>(string endpoint, string[] ids)
        {
            var url = _apiUrl + endpoint;
            if (ids != null && ids.Length > 0)
                url += "/" + string.Join("/", ids);

            var http = Task.Run(() => _client.GetAsync(url)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public IEnumerable<T> HttpGet<T>(string endpoint, Dictionary<string, string> variables)
        {
            var url = _apiUrl + endpoint + "?";
            foreach (var kvp in variables)
                url += $"{kvp.Key}={kvp.Value}&";

            var http = Task.Run(() => _client.GetAsync(url)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<IEnumerable<T>>(json);

            if (http.StatusCode == HttpStatusCode.OK)
            {
                if (http.Headers.TryGetValues(Pagination.HeaderKey, out IEnumerable<string> values))
                {
                    _pagination = JsonSerializer.Deserialize<Pagination>(values.First());
                }
            }

            return response;
        }

        public T HttpPost<T>(string endpoint, object payload)
        {
            var url = _apiUrl + endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var http = Task.Run(() => _client.PostAsync(url, content)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public void HttpPost(string endpoint, object payload)
        {
            var url = _apiUrl + endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var http = Task.Run(() => _client.PostAsync(url, content)).GetAwaiter().GetResult();
            Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter();
        }

        public void HttpDelete(string endpoint, string selector)
        {
            var url = _apiUrl + endpoint + "/" + selector;
            var http = Task.Run(() => _client.DeleteAsync(url)).GetAwaiter().GetResult();
            Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter();
        }

        public static string DictionaryToQueryString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return string.Empty;
            }

            var queryParams = new List<string>();

            foreach (var kvp in dictionary)
            {
                string key = HttpUtility.UrlEncode(kvp.Key);
                string value = HttpUtility.UrlEncode(kvp.Value);
                queryParams.Add($"{key}={value}");
            }

            return string.Join("&", queryParams);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public Pagination Pagination
            => _pagination;
    }
}