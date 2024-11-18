using System;
using System.Runtime.Serialization;

namespace Common.Search
{
    [Serializable]
    public class AmbiguousQueryHandlerException : Exception
    {
        public AmbiguousQueryHandlerException(string name)
            : base($"You cannot define multiple handlers for the same query ({name}).")
        {
        }

        protected AmbiguousQueryHandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}