using System;
using System.Runtime.Serialization;

namespace Tek.Bus
{
    [Serializable]
    public class AmbiguousEventHandlerException : Exception
    {
        public AmbiguousEventHandlerException(string name)
            : base($"You cannot define multiple handlers for the same event ({name}).")
        {
        }

        protected AmbiguousEventHandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}