using System;

namespace Tek.Contract
{
    public class CreateAggregate
    {
        public Guid AggregateId { get; set; }
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class ModifyAggregate
    {
        public Guid AggregateId { get; set; }
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class DeleteAggregate
    {
        public Guid AggregateId { get; set; }
    }
}