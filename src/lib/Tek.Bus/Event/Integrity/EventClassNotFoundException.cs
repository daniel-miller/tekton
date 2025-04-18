using System;
using System.Runtime.Serialization;

namespace Tek.Bus
{
    [Serializable]
    internal class EventClassNotFoundException : Exception
    {
        public EventClassNotFoundException()
        {
        }

        public EventClassNotFoundException(string message) : base(message)
        {
        }

        public EventClassNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EventClassNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}