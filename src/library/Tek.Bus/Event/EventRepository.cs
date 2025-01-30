using System;



namespace Tek.Bus
{
    /// <summary>
    /// Saves and gets aggregates to and from an event store.
    /// </summary>
    public class EventRepository : IEventRepository
    {
        private readonly IEventStore _store;

        public EventRepository(IEventStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        /// <summary>
        /// Gets an aggregate from the event store.
        /// </summary>
        public T Get<T>(Guid aggregate, int? expectedVersion = -1) where T : AggregateRoot
        {
            return Rehydrate<T>(aggregate, expectedVersion);
        }

        public T GetClone<T>(Guid aggregateId, int? version = -1) where T : AggregateRoot
        {
            return Rehydrate<T>(aggregateId, version);
        }

        public void LockAndRun<T>(Guid aggregateId, Action<T> action) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific aggregate as at a specific version.
        /// </summary>
        public T Peek<T>(Guid aggregate, int asAtVersion) where T : AggregateRoot
        {
            return RehydrateAsAt<T>(aggregate, asAtVersion);
        }

        /// <summary>
        /// Returns true if an aggregate exists.
        /// </summary>
        public bool Exists<T>(Guid aggregate)
        {
            return _store.Exists<T>(aggregate);
        }

        /// <summary>
        /// Saves all uncommitted events to the event store.
        /// </summary>
        public IEvent[] Save<T>(T aggregate, int? expectedVersion) where T : AggregateRoot
        {
            if (expectedVersion != null && _store.Exists(aggregate.AggregateIdentifier, expectedVersion.Value))
                throw new ConcurrencyException(aggregate.AggregateIdentifier);

            // Get the list of events that are not yet saved. 
            var events = aggregate.FlushUncommittedEvents();

            if (events.Length > 0) // Save the uncommitted events.
                _store.Save(aggregate, events);

            // The event repository is not responsible for publishing these events. Instead they are returned to the 
            // caller for that purpose.
            return events;
        }

        /// <summary>
        /// Loads an aggregate instance from the full history of events for that aggreate.
        /// </summary>
        private T Rehydrate<T>(Guid id, int? expectedVersion = -1) where T : AggregateRoot
        {
            // Get all the events for the aggregate.
            var events = _store.GetEvents(id, expectedVersion ?? -1);

            // Disallow empty event streams.
            if (events.Length == 0)
                throw new AggregateNotFoundException(typeof(T), id);

            // Create and load the aggregate.
            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.Rehydrate(events);
            return aggregate;
        }

        /// <summary>
        /// Loads an aggregate instance from the full history of events for that aggreate.
        /// </summary>
        private T RehydrateAsAt<T>(Guid id, int version) where T : AggregateRoot
        {
            // Get all the events for the aggregate.
            var events = _store.GetEvents(id, -1, version);

            // Disallow empty event streams.
            if (events.Length == 0)
                throw new AggregateNotFoundException(typeof(T), id);

            // Create and load the aggregate.
            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.Rehydrate(events);
            return aggregate;
        }

        (AggregateState prev, AggregateState current) IEventRepository.GetPrevAndCurrentStates(Guid aggregateId, int version)
            => throw new NotImplementedException();

        #region Methods (boxing and unboxing)

        /// <summary>
        /// Copies an aggregate to offline storage and removes it from online logs.
        /// </summary>
        /// <remarks>
        /// Aggregate boxing/unboxing is not implemented by default for all aggregates. It must be explicitly 
        /// implemented per aggregate for those aggregates that require this functionality, and snapshots are required. 
        /// Therefore this function in this class throws a NotImplementedException; refer to SnapshotRepository for the
        /// implementation.
        /// </remarks>
        public void Box<T>(T aggregate) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves an aggregate from offline storage and returns only its most recent state.
        /// </summary>
        /// <remarks>
        /// Aggregate boxing/unboxing is not implemented by default for all aggregates. It must be explicitly 
        /// implemented per aggregate for those aggregates that require this functionality, and snapshots are required. 
        /// Therefore this function in this class throws a NotImplementedException; refer to SnapshotRepository for the
        /// implementation.
        /// </remarks>
        public T Unbox<T>(Guid aggregateId) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
