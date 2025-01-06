using System;

using Atom.Common;

namespace Atom.Bus
{
    /// <summary>
    /// Provides functions to convert between instances of IQuery and SerializedQuery.
    /// </summary>
    public class QuerySerializer
    {
        private readonly IJsonSerializer _serializer;

        public QuerySerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Returns a deserialized query.
        /// </summary>
        public IQuery<TResult> Deserialize<TResult>(Type queryType, string queryCriteria)
        {
            try
            {
                var queryObject = _serializer.Deserialize<IQuery<TResult>>(queryType, queryCriteria, JsonPurpose.Storage);

                return queryObject;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"{ex.Message} Query: Type = {queryType.Name}, Criteria = [{queryCriteria}]", ex);
            }
        }

        /// <summary>
        /// Returns a deserialized query.
        /// </summary>
        public IQuery<TResult> Deserialize<TResult>(SerializedQuery x)
        {
            try
            {
                var data = _serializer.Deserialize<IQuery<TResult>>(Type.GetType(x.QueryClass), x.QueryCriteria, JsonPurpose.Storage);

                data.OriginShard = x.OriginShard;
                data.OriginActor = x.OriginActor;

                data.QueryIdentifier = x.QueryIdentifier;

                return data;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"{ex.Message} Query: Type = {x.QueryType}, Identifier = {x.QueryIdentifier}, Criteria = [{x.QueryCriteria}]", ex);
            }
        }

        /// <summary>
        /// Returns a serialized query.
        /// </summary>
        public SerializedQuery Serialize<TResult>(IQuery<TResult> query)
        {
            var criteria = _serializer.Serialize(query, JsonPurpose.Storage, false, new[]
            {
                "OriginShard",
                "OriginActor",
                "QueryIdentifier"
            });

            var reflector = new Reflector();

            var serialized = new SerializedQuery
            {
                QueryClass = reflector.GetClassName(query.GetType()),
                QueryType = query.GetType().Name,
                QueryCriteria = criteria,

                QueryIdentifier = query.QueryIdentifier,

                OriginShard = query.OriginShard,
                OriginActor = query.OriginActor
            };

            if (serialized.QueryClass.Length > 200)
                throw new OverflowException($"The assembly-qualified name for this query ({serialized.QueryClass}) exceeds the maximum character limit (200).");

            if (serialized.QueryType.Length > 100)
                throw new OverflowException($"The type name for this query ({serialized.QueryType}) exceeds the maximum character limit (100).");

            if ((serialized.ExecutionStatus?.Length ?? 0) > 20)
                throw new OverflowException($"The execution status ({serialized.ExecutionStatus}) exceeds the maximum character limit (20).");

            return serialized;
        }
    }
}