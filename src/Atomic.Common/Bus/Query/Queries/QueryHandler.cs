using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomic.Common.Bus
{
    /// <summary>
    /// Implements a basic queue for handling queries. The purpose of the queue is to route queries to handler methods; 
    /// validation of a query itself is the responsibility of its subscriber.
    /// </summary>
    public class QueryDispatcher 
    {
        private readonly IEnumerable<IQueryExecutor> _executors;

        public QueryDispatcher(IEnumerable<IQueryExecutor> executors)
        {
            _executors = executors;
        }

        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            var handler = _executors.FirstOrDefault(h => h.CanExecute(query.GetType()));

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler found for query type: {query.GetType().Name}");
            }

            return handler.Execute(query);
        }
    }
}