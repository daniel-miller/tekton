using System;
using System.Net;
using System.Threading.Tasks;

using Tek.Base;

namespace Tek.Contract.Integration.PreMailer
{
    public interface IPreMailerApiClient
    {
        Task<string> Transform(string html);
    }

    public class PreMailerApiClient : IPreMailerApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IJsonSerializer _serializer;

        public PreMailerApiClient(IHttpClientFactory factory, IJsonSerializer serializer)
        {
            _factory = factory;
            _serializer = serializer;
        }

        public async Task<string> Transform(string html)
        {
            using (var client = _factory.Create())
            {
                var url = $"execute";

                var api = new ApiClient(_factory, _serializer);

                var result = await api.HttpPost<string>(url, html, "plain/text");

                if (result.Status != HttpStatusCode.OK)
                    throw new Exception($"Transformation failed. The integration API returned HTTP {result.Status}. {string.Join(" -- ", result.Errors)}");

                return result.Data;
            }
        }
    }
}