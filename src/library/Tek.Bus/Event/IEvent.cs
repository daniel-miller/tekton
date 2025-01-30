using System;

namespace Tek.Bus
{
    /// <summary>
    /// Defines the minimum set of properties that every event must implement.
    /// </summary>
    /// <remarks>
    /// An event represents something that has taken place in the domain. It is always named with a past-participle 
    /// verb, such as Order Confirmed. Since an event represents something in the past, it can be considered a 
    /// statement of fact, which can be used to make decisions in other parts of the system.Events are immutable 
    /// because they represent domain actions that occurred in the past, and the past cannot be altered.
    /// </remarks>
    public interface IEvent
    {
        Guid AggregateIdentifier { get; set; }
        int AggregateVersion { get; set; }

        AggregateState AggregateState { get; set; }

        int OriginShard { get; set; }
        Guid OriginActor { get; set; }

        DateTimeOffset OriginTime { get; set; }
    }
}
