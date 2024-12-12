using System;

namespace Common.Search
{
    public interface IQueryHandler
    {
        TResult Execute<TResult>(IQuery<TResult> query);
        void Subscribe<TQuery, TResult>(Func<TQuery, TResult> handle) where TQuery : IQuery<TResult>;
    }
}