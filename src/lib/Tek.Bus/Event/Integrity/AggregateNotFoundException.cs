using System;
using System.Runtime.Serialization;

namespace Tek.Bus
{
    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type type, Guid id)
            : base($"This aggregate does not exist ({type.FullName} {id}) because there are no events for it.")
        {
        }

        protected AggregateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}