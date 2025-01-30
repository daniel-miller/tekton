using System;
using System.Collections.Generic;

namespace Tek.Bus
{
    /// <summary>
    /// Defines the methods needed from the event store.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Returns the size of an aggregate event stream.
        /// </summary>
        int Count(Guid aggregate, int fromVersion = -1);

        /// <summary>
        /// Returns the number of events that match the search criteria.
        /// </summary>
        int Count(Guid aggregate, string keyword, bool includeChildren);

        /// <summary>
        /// Returns the number of events that match the search criteria.
        /// </summary>
        int Count(string aggregateType, IEnumerable<Guid> aggregateIdentifiers);

        /// <summary>
        /// Returns true if an aggregate exists.
        /// </summary>
        bool Exists<T>(Guid aggregate);

        /// <summary>
        /// Returns true if an aggregate with a specific version exists.
        /// </summary>
        bool Exists(Guid aggregate, int version);

        /// <summary>
        /// Gets all the identifiers for a specific aggregate type.
        /// </summary>
        List<Guid> GetAggregates(string aggregateType);

        /// <summary>
        /// Gets events for an aggregate starting at a specific version. To get all events use version = -1.
        /// </summary>
        IEvent[] GetEvents(Guid aggregate, int fromVersion);

        /// <summary>
        /// Gets events for an aggregate within a specific range of versions.
        /// </summary>
        IEvent[] GetEvents(Guid aggregate, int fromVersion, int toVersion);

        /// <summary>
        /// Gets events for all aggregates of a specific type.
        /// </summary>
        IEvent[] GetEvents(string aggregateType, Guid? id, IEnumerable<string> eventTypes);

        /// <summary>
        /// Gets serialized events for all aggregates of a specific type.
        /// </summary>
        /// <returns></returns>
        List<SerializedEvent> GetSerializedEventsPaged(Guid aggregate, string keyword, bool includeChildren, int skip, int pageSize);

        /// <summary>
        /// Enumerate events for all aggregates of a specific type.
        /// </summary>
        List<IEvent> GetEvents(string aggregateType, IEnumerable<Guid> aggregateIdentifiers);

        /// <summary>
        /// Returns the events that match the search criteria.
        /// </summary>
        IEvent[] GetEventsPaged(Guid aggregate, string keyword, bool includeChildren, int skip, int pageSize);

        /// <summary>
        /// Gets a specific event from an aggregate event stream.
        /// </summary>
        IEvent GetEvent(Guid aggregate, int version);

        /// <summary>
        /// Gets all aggregates that are scheduled to expire at (or before) a specific time on a specific date.
        /// </summary>
        Guid[] GetExpired(DateTimeOffset at);

        /// <summary>
        /// Save eventss.
        /// </summary>
        void Save(AggregateRoot aggregate, IEnumerable<IEvent> events);
        void Save(IEnumerable<AggregateImport> import);

        /// <summary>
        /// Save event.
        /// </summary>
        void Save(IEvent @event);

        /// <summary>
        /// Insert event into the stream at a specific position.
        /// </summary>
        /// <remarks>
        /// Aggregate event streams index from starting position 1 (not 0).
        /// </remarks>
        void Insert(IEvent @event, int index);

        /// <summary>
        /// Copies an aggregate to offline storage and removes it from online logs.
        /// </summary>
        /// <remarks>
        /// Someone who is a purist with regard to event sourcing will red-flag this function and say the event stream 
        /// for an aggregate should never be altered or removed. However, we have two scenarios in which this is a non-
        /// negotiable business requirement. First, when a customer does not renew their contract with our business, we
        /// have a contractual obligation to remove all the customer's data from our systems. Second, we frequently run
        /// test-cases to confirm system functions are operating correctly; this data is temporary by definition, and 
        /// we do not want to permanently store the event streams for test aggregates.
        /// </remarks>
        void Box(Guid aggregate, bool archive);
        void Unbox(Guid aggregate, Func<Guid, AggregateRoot> creator);

        /// <summary>
        /// Performs a rollback on a specific aggregate to a specific version number. In simplest terms, this method deletes
        /// all the events in an aggregate where the version number is greater than or equal to the input parameter.
        /// </summary>
        int Rollback(Guid id, int version);

        /// <summary>
        /// Allows to mark a event as an obsolete
        /// EventStore doesn't try to deserialize an obsolete event and returns the event as SerializedEvent
        /// This is useful when a specific event is obsolete and we need either ignore it or transform to a new event
        /// </summary>
        void RegisterObsoleteEventTypes(IEnumerable<string> eventTypes);

        void GetClassAndOrganization(Guid aggregate, out string @class, out Guid organization);
    }
}
