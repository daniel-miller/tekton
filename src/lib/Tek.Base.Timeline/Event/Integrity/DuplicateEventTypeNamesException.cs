using System;
using System.Runtime.Serialization;

namespace Tek.Base.Timeline
{
    [Serializable]
    internal class DuplicateEventTypeNamesException : Exception
    {
        public DuplicateEventTypeNamesException()
        {
        }

        public DuplicateEventTypeNamesException(string message) : base(message)
        {
        }

        public DuplicateEventTypeNamesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateEventTypeNamesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}