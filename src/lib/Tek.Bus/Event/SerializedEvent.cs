using System;

namespace Tek.Bus
{
    /// <summary>
    /// Provides a serialization wrapper for events so that common properties are not embedded inside the event data.
    /// </summary>
    public class SerializedEvent : IEvent
    {
        /// <summary>
        /// Identifies the aggregate for which the event was raised.
        /// </summary>
        public Guid AggregateIdentifier { get; set; }

        /// <summary>
        /// Version number of the aggregate for which the event was raised.
        /// </summary>
        public int AggregateVersion { get; set; }

        /// <summary>
        /// An event may (optionally) include the current state for the aggregate that raised it.
        /// </summary>
        /// <remarks>
        /// Event subscribers must never assume this property is used, therefore it may be null some (or all!) of the 
        /// time. Each individual command subscriber decides for itself whether or not to set this property before it
        /// publishes new events. It is strictly for convenience, when you have event subscribers, process managers,
        /// and/or query projections that need to work with the aggregate state "as at" the time of a published event.
        /// </remarks>
        public AggregateState AggregateState { get; set; }

        /// <summary>
        /// Identifies the organization for the session in which the event was raised.
        /// </summary>
        public int OriginShard { get; set; }

        /// <summary>
        /// Identifies the user for the session in which the event was raised.
        /// </summary>
        public Guid OriginActor { get; set; }

        /// <summary>
        /// Abbreviated class name.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Serialized data for the event.
        /// </summary>
        public string EventData { get; set; }

        /// <summary>
        /// Date and time the event was raised.
        /// </summary>
        public DateTimeOffset OriginTime { get; set; }

        /// <summary>
        /// Constructs a new instance. By default the event time is now.
        /// </summary>
        public SerializedEvent()
        {
            OriginTime = DateTimeOffset.UtcNow;
        }
    }
}
