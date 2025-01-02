using System.Threading.Tasks;

using Atomic.Common.Bus;

namespace Atomic.Common
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

        public async Task ExecuteCommand<TResult>(ICommand command)
        {
            var serialized = _serializer.Serialize(command);
            await _client.HttpPost("commands/execute", serialized);
        }

        public async Task<TResult> RunQuery<TResult>(IQuery<TResult> query)
        {
            return await _client.HttpPost<TResult>("queries/run", query);
        }

        public async Task PublishEvent<TResult>(IEvent @event)
        {
            var serialized = _serializer.Serialize(@event);
            await _client.HttpPost("events/publish", serialized);
        }
    }
}