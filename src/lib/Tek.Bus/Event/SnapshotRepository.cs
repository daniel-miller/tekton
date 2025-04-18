using System;
using System.Linq;

using Tek.Common;
using Tek.Contract;

namespace Tek.Bus
{
    /// <summary>
    /// Saves and gets snapshots to and from a snapshot store.
    /// </summary>
    public class SnapshotRepository : IEventRepository
    {
        private readonly GuidCache<AggregateRoot> _cache;
        private readonly IJsonSerializer _serializer;

        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotStrategy _snapshotStrategy;
        private readonly IEventRepository _eventRepository;
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Constructs a new SnapshotRepository instance.
        /// </summary>
        /// <param name="eventStore">Store where events are persisted</param>
        /// <param name="eventRepository">Repository to get aggregates from the event store</param>
        /// <param name="snapshotStore">Store where snapshots are persisted</param>
        /// <param name="snapshotStrategy">Strategy used to determine when to take a snapshot</param>
        public SnapshotRepository(IEventStore eventStore, IEventRepository eventRepository, 
            ISnapshotStore snapshotStore, ISnapshotStrategy snapshotStrategy, 
            IJsonSerializer serializer)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));

            _snapshotStore = snapshotStore ?? throw new ArgumentNullException(nameof(snapshotStore));
            _snapshotStrategy = snapshotStrategy ?? throw new ArgumentNullException(nameof(snapshotStrategy));

            _cache = new GuidCache<AggregateRoot>();
            _serializer = serializer;
        }

        /// <summary>
        /// Saves the aggregate. Takes a snapshot if needed.
        /// </summary>
        public IEvent[] Save<T>(T aggregate, int? version = null) where T : AggregateRoot
        {
            var concurrencyEvent = false;

            var previous = (T)_cache.Get(aggregate.AggregateIdentifier);

            if (previous != null && aggregate != previous)
                throw new ConcurrencyException($"Aggregate {aggregate.AggregateIdentifier} version {aggregate.AggregateVersion} cannot be saved because another aggregate (version {aggregate.AggregateVersion}) already exists in the cache with the same identifier. Your code might be trying to create a new aggregate that is already created.");

            // Cache the aggregate for 5 minutes.
            lock (_cache)
                _cache.Add(aggregate.AggregateIdentifier, aggregate, 5 * 60, true);

            IEvent[] events = null;

            aggregate.LockAndRun(() =>
            {
                if (!concurrencyEvent)
                {
                    // Take a snapshot if needed but only if no concurrency events happened
                    TakeSnapshot(aggregate, false);
                }

                // Return the stream of saved events to the caller so they can be published.
                events = _eventRepository.Save(aggregate, version);
            });

            return events;
        }

        /// <summary>
        /// Gets the aggregate.
        /// </summary>
        public T Get<T>(Guid aggregateId, int? version = -1) where T : AggregateRoot
        {
            T aggregate;

            lock (_cache)
            {
                aggregate = (T)_cache.Get(aggregateId);

                if (aggregate == null)
                    aggregate = CreateAggregate<T>(aggregateId, null);

                _cache.Add(aggregate.AggregateIdentifier, aggregate, 5 * 60, true);
            }

            return aggregate;
        }

        public T GetClone<T>(Guid aggregateId, int? expectedVersion = -1) where T : AggregateRoot
        {
            var aggregate = Get<T>(aggregateId);
            if (aggregate != null)
            {
                T clone = default;

                aggregate.LockAndRun(() =>
                {
                    var originalState = _serializer.Serialize(aggregate.State);

                    clone = AggregateFactory<T>.CreateAggregate();
                    clone.AggregateIdentifier = aggregate.AggregateIdentifier;
                    clone.RootAggregateIdentifier = aggregate.RootAggregateIdentifier;
                    clone.AggregateVersion = aggregate.AggregateVersion;
                    clone.State = _serializer.Deserialize<AggregateState>(aggregate.State.GetType(), originalState, JsonPurpose.Storage);
                });

                return clone;
            }

            return CreateAggregate<T>(aggregateId, expectedVersion);
        }

        public void LockAndRun<T>(Guid aggregateId, Action<T> action) where T : AggregateRoot
        {
            var aggregate = Get<T>(aggregateId, null);

            aggregate.LockAndRun(() =>
            {
                action?.Invoke(aggregate);
            });
        }

        private T CreateAggregate<T>(Guid aggregateId, int? version) where T : AggregateRoot
        {
            // If it is not in the cache then load the aggregate from the most recent snapshot.
            var aggregate = AggregateFactory<T>.CreateAggregate();
            var snapshotVersion = RestoreAggregateFromSnapshot(aggregateId, aggregate);

            // If there is no snapshot then load the aggregate directly from the event store.
            if (snapshotVersion == (version ?? -1))
                return _eventRepository.Get<T>(aggregateId);

            // Otherwise load the aggregate from the events that occurred after the snapshot was taken.
            var events = _eventStore.GetEvents(aggregateId, snapshotVersion)
                .Where(desc => desc.AggregateVersion > snapshotVersion);

            aggregate.Rehydrate(events);

            return aggregate;
        }

        public (AggregateState prev, AggregateState current) GetPrevAndCurrentStates(Guid aggregateId, int version)
        {           
            var events = _eventStore.GetEvents(aggregateId, -1, version);
            if (events.Length == 0)
                return (null, null);

            _eventStore.GetClassAndOrganization(aggregateId, out var aggregateClass, out var _);

            var aggregateType = Type.GetType(aggregateClass);
            var aggregate = (AggregateRoot)aggregateType.GetConstructor(Type.EmptyTypes).Invoke(null);

            var prevState = aggregate.CreateState();
            for (int i = 0; i < events.Length - 1; i++)
                prevState.Apply(events[i]);

            var prevStateJson = _serializer.Serialize(prevState);

            var currentState = _serializer.Deserialize<AggregateState>(prevState.GetType(), prevStateJson, JsonPurpose.Storage);
            currentState.Apply(events[events.Length - 1]);

            if (events.Length == 1)
                prevState = null;

            return (prevState, currentState);
        }

        /// <summary>
        /// Returns a specific aggregate as at a specific version.
        /// </summary>
        public T Peek<T>(Guid _, int __) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if an aggregate exists.
        /// </summary>
        public bool Exists<T>(Guid aggregate)
        {
            return _eventRepository.Exists<T>(aggregate);
        }

        /// <summary>
        /// Loads the aggregate from the most recent snapshot.
        /// </summary>
        /// <returns>
        /// Returns the version number for the aggregate when the snapshot was taken.
        /// </returns>
        private int RestoreAggregateFromSnapshot<T>(Guid id, T aggregate) where T : AggregateRoot
        {
            var snapshot = _snapshotStore.Get(id);

            if (snapshot == null)
                return -1;

            aggregate.AggregateIdentifier = snapshot.AggregateIdentifier;
            aggregate.AggregateVersion = snapshot.AggregateVersion;
            aggregate.State = _serializer.Deserialize<AggregateState>(aggregate.CreateState().GetType(), snapshot.AggregateState, JsonPurpose.Storage);

            return snapshot.AggregateVersion;
        }

        /// <summary>
        /// Saves a snapshot of the aggregate if the strategy indicates a snapshot should now be taken.
        /// </summary>
        private void TakeSnapshot(AggregateRoot aggregate, bool force)
        {
            var count = _snapshotStore.Count(aggregate.AggregateIdentifier);

            if (!force && !_snapshotStrategy.ShouldTakeSnapShot(aggregate, count))
                return;

            var snapshot = new Snapshot
            {
                AggregateIdentifier = aggregate.AggregateIdentifier,
                AggregateVersion = aggregate.AggregateVersion
            };

            SerializeAggregateState(aggregate, snapshot);

            snapshot.AggregateVersion = aggregate.AggregateVersion + aggregate.GetUncommittedEvents().Length;

            _snapshotStore.Save(snapshot);
        }

        private void SerializeAggregateState(AggregateRoot aggregate, Snapshot snapshot)
        {
            try
            {
                snapshot.AggregateState = _serializer.Serialize(aggregate.State);
            }
            catch (InvalidOperationException ex)
            {
                var type = aggregate.GetType().Name;
                var id = aggregate.AggregateIdentifier;
                var version = aggregate.AggregateVersion;
                var message = $"Serialization failed for {type} {id} version {version}.";
                throw new SnapshotSerializationFailedException(message, ex);
            }
        }

        #region Methods (boxing and unboxing)

        /// <summary>
        /// Checks for expired aggregates. Automatically boxes all aggregates for which the timer is now elapsed.
        /// </summary>
        public void Ping()
        {
            var aggregates = _eventStore.GetExpired(DateTimeOffset.UtcNow);
            foreach (var aggregate in aggregates)
                Box(Get<AggregateRoot>(aggregate));
        }

        /// <summary>
        /// Copies an aggregate to offline storage and removes it from online logs.
        /// </summary>
        public void Box<T>(T aggregate) where T : AggregateRoot
        {
            TakeSnapshot(aggregate, true);

            _snapshotStore.Box(aggregate.AggregateIdentifier);
            _eventStore.Box(aggregate.AggregateIdentifier, true);

            _cache.Remove(aggregate.AggregateIdentifier);
        }

        /// <summary>
        /// Retrieves an aggregate from offline storage and returns only its most recent state.
        /// </summary>
        public T Unbox<T>(Guid aggregateId) where T : AggregateRoot
        {
            var snapshot = _snapshotStore.Unbox(aggregateId);
            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.AggregateIdentifier = aggregateId;
            aggregate.AggregateVersion = 1;
            aggregate.State = _serializer.Deserialize<AggregateState>(aggregate.CreateState().GetType(), snapshot.AggregateState, JsonPurpose.Storage);
            return aggregate;
        }

        #endregion
    }
}