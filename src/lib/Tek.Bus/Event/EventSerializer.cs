using System;

using Tek.Contract;

namespace Tek.Bus
{
    /// <summary>
    /// Provides functions to convert between instances of IEvent and SerializedEvent.
    /// </summary>
    public class EventSerializer
    {
        private readonly IJsonSerializer _serializer;

        public EventSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Returns a deserialized event.
        /// </summary>
        public IEvent Deserialize(SerializedEvent x)
        {
            var type = AggregateRegistry.GetEventType(x.EventType)
                ?? throw new EventNotFoundException(x.EventType);

            var data = _serializer.Deserialize<IEvent>(type, x.EventData, JsonPurpose.Storage);

            CopyEventProperties(x, data);

            return data;
        }

        /// <summary>
        /// Returns a deserialized event.
        /// </summary>
        public T Deserialize<T>(SerializedEvent x) where T: IEvent
        {
            var data = _serializer.Deserialize<T>(typeof(T), x.EventData, JsonPurpose.Storage);

            CopyEventProperties(x, data);

            return data;
        }

        private void CopyEventProperties(SerializedEvent source, IEvent dest)
        {
            dest.AggregateIdentifier = source.AggregateIdentifier;
            dest.AggregateVersion = source.AggregateVersion;
            dest.OriginTime = source.OriginTime;
            dest.OriginShard = source.OriginShard;
            dest.OriginActor = source.OriginActor;
        }

        /// <summary>
        /// Returns a serialized event.
        /// </summary>
        public SerializedEvent Serialize(IEvent @event, Guid aggregateIdentifier, int version)
        {
            var data = _serializer.Serialize(@event
                , JsonPurpose.Storage
                , false
                , new[] { "AggregateIdentifier", "AggregateState", "AggregateVersion", "OriginShard", "OriginTime", "OriginActor" });

            var serialized = new SerializedEvent
            {
                AggregateIdentifier = aggregateIdentifier,
                AggregateVersion = version,

                OriginTime = @event.OriginTime,
                EventType = @event.GetType().Name,
                EventData = data,

                OriginShard = @event.OriginShard,
                OriginActor = @event.OriginActor
            };

            @event.OriginShard = serialized.OriginShard;
            @event.OriginActor = serialized.OriginActor;

            return serialized;
        }
    }
}