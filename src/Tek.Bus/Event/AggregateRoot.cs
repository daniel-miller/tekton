using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace Tek.Bus
{
    /// <summary>
    /// Implements the base class for all aggregate roots. An aggregate forms a tree or graph of object relations. The 
    /// aggregate root is the top-level container, which speaks for the whole and may delegates down to the rest. It is 
    /// important because it is the one that the rest of the world communicates with.
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Events to the state of the aggregate that are not yet committed to a persistent event store.
        /// </summary>
        private readonly List<IEvent> _events = new List<IEvent>();

        /// <summary>
        /// Represents the state (i.e. data/packet) for the aggregate.
        /// </summary>
        public AggregateState State { get; set; }

        /// <summary>
        /// Uniquely identifies the aggregate.
        /// </summary>
        public Guid AggregateIdentifier { get; set; }

        /// <summary>
        /// Uniquely identifies the aggregate.
        /// </summary>
        public Guid RootAggregateIdentifier { get; set; }

        /// <summary>
        /// Current version of the aggregate.
        /// </summary>
        public int AggregateVersion { get; set; }

        /// <summary>
        /// Every aggregate must override this method to create the object that holds its current state.
        /// </summary>
        public abstract AggregateState CreateState();

        /// <summary>
        /// Returns all uncommitted events. 
        /// </summary>
        /// <returns></returns>
        public IEvent[] GetUncommittedEvents()
        {
            IEvent[] events = null;

            LockAndRun(() =>
            {
                events = _events.ToArray();
            });

            return events;
        }

        /// <summary>
        /// Returns all uncommitted events and clears them from the aggregate.
        /// </summary>
        public IEvent[] FlushUncommittedEvents()
        {
            IEvent[] events = null;

            LockAndRun(() =>
            {
                events = _events.ToArray();

                var i = 0;

                foreach (var @event in events)
                {
                    if (@event.AggregateIdentifier == Guid.Empty && AggregateIdentifier == Guid.Empty)
                        throw new MissingAggregateIdentifierException(GetType(), @event.GetType());

                    if (@event.AggregateIdentifier == Guid.Empty)
                        @event.AggregateIdentifier = AggregateIdentifier;

                    i++;

                    @event.AggregateVersion = AggregateVersion + i;
                    @event.OriginTime = DateTimeOffset.UtcNow;
                }

                AggregateVersion += events.Length;

                _events.Clear();
            });

            return events;
        }

        /// <summary>
        /// Assigns a specific organization and user identity to every uncomitted event.
        /// </summary>
        public void Identify(int shard, Guid actor)
        {
            LockAndRun(() =>
            {
                foreach (var @event in _events)
                {
                    if (@event.OriginShard == 0)
                        @event.OriginShard = shard;

                    if (@event.OriginActor == Guid.Empty)
                        @event.OriginActor = actor;
                }
            });
        }

        /// <summary>
        /// Loads an aggregate from a stream of events.
        /// </summary>
        public void Rehydrate(IEnumerable<IEvent> events)
        {
            LockAndRun(() =>
            {
                foreach (var @event in events.ToArray())
                {
                    if (@event.AggregateVersion != AggregateVersion + 1)
                        throw new UnorderedEventsException(@event.AggregateIdentifier);

                    ApplyEvent(@event);

                    AggregateIdentifier = @event.AggregateIdentifier;
                    AggregateVersion++;
                }
            });
        }

        /// <summary>
        /// Applies an event to the aggregate state AND appends the event to the history of uncommited events.
        /// </summary>
        public void Apply(IEvent @event)
        {
            if (@event.AggregateIdentifier == Guid.Empty)
                @event.AggregateIdentifier = AggregateIdentifier;

            LockAndRun(() =>
            {
                ApplyEvent(@event);
                _events.Add(@event);
            });
        }

        /// <summary>
        /// Applies an event to the aggregate state. This method is called internally when rehydrating an aggregate, 
        /// and you can override this when custom handling is needed.
        /// </summary>
        private void ApplyEvent(IEvent @event)
        {
            if (State == null)
                State = CreateState();

            State.Apply(@event);
        }

        public void LockAndRun(Action action)
        {
            // LockEnter
            {
                const int LockTimeoutInMs = 2 * 1000; // 2 sec
                const int LockWaitInMs = 100;
                const int LockTryCount = 50;

                var tryCount = LockTryCount;

                while (!Monitor.TryEnter(this, LockTimeoutInMs))
                {
                    if (--tryCount <= 0)
                        throw new ConcurrencyException($"The aggregate {AggregateIdentifier} cannot be locked by the current thread.");

                    Thread.Sleep(LockWaitInMs);
                }
            }

            try
            {
                action?.Invoke();
            }
            finally
            {
                // LockExit
                {
                    Monitor.Exit(this);
                }
            }
        }
    }
}
