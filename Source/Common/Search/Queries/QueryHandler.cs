using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Search
{
    /// <summary>
    /// Implements a basic query queue. The purpose of the queue is to route queries to handler methods; validation of
    /// a query itself is the responsibility of its handler.
    /// </summary>
    public class QueryHandler : IQueryHandler
    {
        private readonly Reflector _reflector = new Reflector();

        /// <summary>
        /// A query's full class name is the key to find the method that handles it.
        /// </summary>
        private readonly Dictionary<string, Delegate> _handlers = new Dictionary<string, Delegate>();

        public QueryHandler()
        {
            
        }

        private TResult Execute<TResult>(IQuery<TResult> query, string @class)
        {
            if (_handlers.ContainsKey(@class))
            {
                var action = _handlers[@class];
                return (TResult)action.DynamicInvoke(query);
            }

            return default;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return Execute(query, _reflector.GetClassName(query.GetType()));
        }

        public void Subscribe<TQuery, TResult>(Func<TQuery, TResult> handle) where TQuery : IQuery<TResult>
        {
            var name = _reflector.GetClassName(typeof(TQuery));

            if (_handlers.Any(x => x.Key == name))
                throw new AmbiguousQueryHandlerException(name);

            _handlers.Add(name, handle);
        }
    }
}