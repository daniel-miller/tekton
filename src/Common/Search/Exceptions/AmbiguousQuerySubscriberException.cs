using System;
using System.Runtime.Serialization;

namespace Common.Search
{
    [Serializable]
    public class AmbiguousQuerySubscriberException : Exception
    {
        public AmbiguousQuerySubscriberException(string name)
            : base($"You cannot define multiple subscribers for the same query ({name}).")
        {
        }

        protected AmbiguousQuerySubscriberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class MissingQuerySubscriberException : Exception
    {
        public MissingQuerySubscriberException(string name)
            : base($"There is no subscriber for this query ({name}).")
        {
        }

        protected MissingQuerySubscriberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}