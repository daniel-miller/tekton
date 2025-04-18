﻿using System;
using System.Linq.Expressions;

namespace Tek.Bus
{
    /// <summary>
    /// Creates a new aggregate instance using the default constructor.
    /// </summary>
    public static class AggregateFactory<T>
    {
        private static readonly Func<T> Constructor = CreateTypeConstructor();

        public static T CreateAggregate()
        {
            if (Constructor == null)
                throw new MissingDefaultConstructorException(typeof(T));

            return Constructor();
        }

        private static Func<T> CreateTypeConstructor()
        {
            try
            {
                var expr = Expression.New(typeof(T));

                var func = Expression.Lambda<Func<T>>(expr);

                return func.Compile();
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
