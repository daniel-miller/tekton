using System;

namespace Common.Observation
{
    /// <summary>
    /// Provides the features for a basic service bus to handle the publication of events.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes an event to registered subscribers.
        /// </summary>
        void Publish(IEvent @event);

        /// <summary>
        /// Registers a handler for a specific event.
        /// </summary>
        void Subscribe<T>(Action<T> action) where T : IEvent;

        /// <summary>
        /// Register a custom organization-specific handler for the event. 
        /// </summary>
        void Extend<T>(Action<T> action, Guid organization, bool before = false) where T : IEvent;

        /// <summary>
        /// Returns true if a custom organization-specific handler is registered for the event.
        /// </summary>
        bool Extends<T>(Guid organization);

        /// <summary>
        /// Register a custom organization-specific handler for the event. 
        /// </summary>
        void Override<T>(Action<T> action, Guid organization) where T : IEvent;
    }

    public interface IEventProcessor
    {
        void Register(IEventPublisher publisher);
    }
}