using System;

using Tek.Common;

namespace Tek.Bus
{
    public class AggregateTypeInfo : IAggregateTypeInfo
    {
        public Guid ID { get; }
        public Type Type { get; }
        public string Name { get; }

        public IEventTypeInfo[] Events { get; set; }

        public AggregateTypeInfo(Type t)
        {
            const string postfix = "Aggregate";

            ID = UuidFactory.Create();
            Type = t;
            Name = t.Name.EndsWith(postfix)
                ? t.Name.Substring(0, t.Name.Length - postfix.Length)
                : t.Name;
        }

        public override string ToString() => Type.Name;
    }
}
