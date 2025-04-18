using System;
using System.Collections.Generic;
using System.Linq;

using Tek.Common;

namespace Tek.Bus
{
    /// <summary>
    /// Implements a basic event queue.
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly Reflector _reflector;

        /// <summary>
        /// An event's full class name is used as the key to a list of event-handling methods.
        /// </summary>
        private readonly Dictionary<string, List<Action<IEvent>>> _subscribers;

        /// <summary>
        /// In a multi-organization system we may want to allow each individual organization to extend the handling of 
        /// an event. The class name and the organization identifier is used as the unique key here.
        /// </summary>
        private readonly Dictionary<(string, int), Action<IEvent>> _precursors, _extenders;

        /// <summary>
        /// In a multi-organization system we may want to allow each individual organization to override/customize the handling of 
        /// an event. The class name and the organization identifier is used as the unique key here.
        /// </summary>
        private readonly Dictionary<(string, int), Action<IEvent>> _overriders;

        /// <summary>
        /// Constructs the queue.
        /// </summary>
        public EventPublisher()
        {
            _subscribers = new Dictionary<string, List<Action<IEvent>>>();
            _precursors = new Dictionary<(string, int), Action<IEvent>>();
            _extenders = new Dictionary<(string, int), Action<IEvent>>();
            _overriders = new Dictionary<(string, int), Action<IEvent>>();

            _reflector = new Reflector();
        }

        /// <summary>
        /// Invokes each subscriber method registered to handle the event.
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IEvent @event)
        {
            var name = _reflector.GetClassName(@event.GetType());

            var precursorExists = _precursors.ContainsKey((name, @event.OriginShard));
            var subscriberExists = _subscribers.ContainsKey(name);
            var extenderExists = _extenders.ContainsKey((name, @event.OriginShard));

            if (_overriders.ContainsKey((name, @event.OriginShard)))
            {
                var overrider = _overriders[(name, @event.OriginShard)];
                overrider?.Invoke(@event);
            }
            else if (precursorExists || subscriberExists || extenderExists)
            {
                if (precursorExists)
                {
                    var precursor = _precursors[(name, @event.OriginShard)];
                    precursor?.Invoke(@event);
                }

                if (subscriberExists)
                {
                    var actions = _subscribers[name];
                    foreach (var action in actions)
                        action.Invoke(@event);
                }

                if (extenderExists)
                {
                    var extender = _extenders[(name, @event.OriginShard)];
                    extender?.Invoke(@event);
                }
            }
            else
            {
                throw new UnhandledEventException(name);
            }
        }

        /// <summary>
        /// Any number of subscribers can register for an event, and any one subscriber can register any number of
        /// methods to be invoked when the event is published. 
        /// </summary>
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            var name = _reflector.GetClassName(typeof(T));

            if (!_subscribers.Any(x => x.Key == name))
                _subscribers.Add(name, new List<Action<IEvent>>());

            _subscribers[name].Add((@event) => action((T)@event));
        }

        /// <summary>
        /// Register a custom organization-specific handler for the event.
        /// </summary>
        public void Extend<T>(Action<T> action, int shard, bool before = false) where T : IEvent
        {
            var name = _reflector.GetClassName(typeof(T));

            if (before)
            {
                if (_precursors.Any(x => x.Key.Item1 == name && x.Key.Item2 == shard))
                    throw new AmbiguousEventHandlerException(name);
                _precursors.Add((name, shard), (command) => action((T)command));
            }
            else
            {
                if (_extenders.Any(x => x.Key.Item1 == name && x.Key.Item2 == shard))
                    throw new AmbiguousEventHandlerException(name);
                _extenders.Add((name, shard), (command) => action((T)command));
            }
        }

        public bool Extends<T>(int shard)
            => _extenders.Any(x => x.Key.Item1 == _reflector.GetClassName(typeof(T)) && x.Key.Item2 == shard);

        /// <summary>
        /// Register a custom organization-specific handler for the event.
        /// </summary>
        public void Override<T>(Action<T> action, int shard) where T : IEvent
        {
            var name = _reflector.GetClassName(typeof(T));

            if (_overriders.Any(x => x.Key.Item1 == name && x.Key.Item2 == shard))
                throw new AmbiguousEventHandlerException(name);

            _overriders.Add((name, shard), (command) => action((T)command));
        }
    }
}
