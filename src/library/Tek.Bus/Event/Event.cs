using System;

namespace Tek.Bus
{
    /// <summary>
    /// Defines the base class for all events.
    /// </summary>
    /// <remarks>
    /// An event represents something that has taken place in the domain. It is always named with a past-participle 
    /// verb, such as Order Confirmed. Since an event represents something in the past, it can be considered a 
    /// statement of fact, which can be used to make decisions in other parts of the system.Events are immutable 
    /// because they represent domain actions that occurred in the past, and the past cannot be altered.
    /// </remarks>
    [Serializable]
    public class Event : IEvent
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
        /// Fully-qualified assembly name for the class that implements the event.
        /// </summary>
        public string EventClass { get; set; }

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
        public Event()
        {
            OriginTime = DateTimeOffset.UtcNow;
        }
    }
}
