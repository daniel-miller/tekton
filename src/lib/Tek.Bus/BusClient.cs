using System.Threading.Tasks;

using Tek.Common;
using Tek.Contract;

namespace Tek.Bus
{
    public class BusClient
    {
        private readonly ApiClient _client;
        private readonly IJsonSerializer _serializer;

        public BusClient(ApiClient client, IJsonSerializer serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public async Task<ApiResult> ExecuteCommand(ICommand command)
        {
            var serialized = _serializer.Serialize(command);
            return await _client.HttpPost("commands/execute", serialized);
        }

        public async Task<ApiResult<QueryResult>> RunQuery<QueryResult>(IQuery<QueryResult> query)
        {
            return await _client.HttpPost<QueryResult>("queries/run", query);
        }

        public async Task<ApiResult> PublishEvent(IEvent @event)
        {
            var serialized = _serializer.Serialize(@event);
            return await _client.HttpPost("events/publish", serialized);
        }
    }
}