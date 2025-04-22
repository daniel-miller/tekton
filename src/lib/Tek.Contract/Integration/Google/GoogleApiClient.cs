using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Tek.Base;

namespace Tek.Contract.Integration.Google
{
    public interface IGoogleApiClient
    {
        Task Translate(string from, string to, IEnumerable<Dictionary<string, string>> values);
        Task Translate(string from, string to, Dictionary<string, string> value);
        Task<string> Translate(string from, string to, string value);
        Task<string[]> Translate(string from, string to, string[] inputs);
    }

    public class GoogleApiClient : IGoogleApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IJsonSerializer _serializer;

        public GoogleApiClient(IHttpClientFactory factory, IJsonSerializer serializer)
        {
            _factory = factory;
            _serializer = serializer;
        }

        public async Task Translate(string from, string to, IEnumerable<Dictionary<string, string>> values)
        {
            var results = await Translate(from, to, values.Select(x => x[from]).ToArray());
            var index = 0;
            foreach (var value in values)
                value[to] = results[index++];
        }

        public async Task Translate(string from, string to, Dictionary<string, string> dictionary)
        {
            dictionary[to] = await Translate(from, to, dictionary[from]);
        }

        public async Task<string> Translate(string from, string to, string value)
        {
            return (await Translate(from, to, new[] { value })).First();
        }

        public async Task<string[]> Translate(string from, string to, string[] inputs)
        {
            using (var client = _factory.Create())
            {
                var url = $"content/translations/translate?from={from}&to={to}";

                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpPost<string[]>(url, inputs);

                if (result.Status != HttpStatusCode.OK)
                    throw new Exception($"Translation failed. The integration API returned HTTP {result.Status}. {string.Join(" -- ", result.Errors)}");

                var outputs = result.Data;

                for (var i = 0; i < outputs.Length; i++)
                {
                    var text = outputs[i];
                    outputs[i] = !string.IsNullOrEmpty(text) ? text.Replace("! [", "![").Replace("] (", "](") : text;
                }

                if (inputs.Length != outputs.Length)
                    throw new Exception($"Translation Failed: The input array contains {inputs.Length} items, but the output array contains {outputs.Length} items.");

                return outputs;
            }
        }
    }
}