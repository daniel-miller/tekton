using System;
using System.Collections.Generic;

namespace Atom.Bus
{
    public interface IQueryExecutor
    {
        TResult Execute<TResult>(IQuery<TResult> query);

        bool CanExecute(Type queryType);
    }

    public class QueryExecutor : IQueryExecutor
    {
        private readonly Dictionary<Type, Func<object, object>> _methods;

        public QueryExecutor()
        {
            _methods = new Dictionary<Type, Func<object, object>>();
        }

        public bool CanExecute(Type queryType)
        {
            return _methods.ContainsKey(queryType);
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            if (_methods.TryGetValue(query.GetType(), out var executor))
            {
                var result = executor(query);
                return (TResult)result;
            }

            throw new InvalidOperationException($"No executor method found for query type {query.GetType().Name}");
        }

        protected void RegisterQuery<T>(Func<object, object> queryMethod)
        {
            _methods.Add(typeof(T), queryMethod);
        }
    }
}