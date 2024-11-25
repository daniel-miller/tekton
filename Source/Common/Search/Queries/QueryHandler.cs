using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Search
{
    /// <summary>
    /// Implements a basic queue for handling queries. The purpose of the queue is to route queries to handler methods; 
    /// validation of a query itself is the responsibility of its subscriber.
    /// </summary>
    public class QueryHandler : IQueryHandler
    {
        private readonly Reflector _reflector = new Reflector();

        /// <summary>
        /// A query's full class name is the key to find the method that subscribes to it.
        /// </summary>
        private readonly Dictionary<string, Delegate> _subscribers = new Dictionary<string, Delegate>();

        public QueryHandler()
        {
            
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return Execute(query, _reflector.GetClassName(query.GetType()));
        }

        public void Subscribe<TQuery, TResult>(Func<TQuery, TResult> handle) where TQuery : IQuery<TResult>
        {
            var name = _reflector.GetClassName(typeof(TQuery));

            if (_subscribers.Any(x => x.Key == name))
                throw new AmbiguousQuerySubscriberException(name);

            _subscribers.Add(name, handle);
        }

        private TResult Execute<TResult>(IQuery<TResult> query, string @class)
        {
            if (!_subscribers.ContainsKey(@class))
                throw new MissingQuerySubscriberException(@class);

            var action = _subscribers[@class];

            return (TResult)action.DynamicInvoke(query);
        }
    }
}