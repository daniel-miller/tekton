using System;

namespace Tek.Bus
{
    /// <summary>
    /// Provides a serialization wrapper for commands.
    /// </summary>
    public class SerializedCommand : ICommand
    {
        public Guid AggregateIdentifier { get; set; }
        public int? ExpectedVersion { get; set; }

        public int OriginShard { get; set; }
        public Guid OriginActor { get; set; }

        public string CommandClass { get; set; }
        public string CommandType { get; set; }

        public string CommandData { get; set; }
        public string CommandDescription { get; set; }

        public Guid CommandIdentifier { get; set; }

        public DateTimeOffset? ExecutionScheduled { get; set; }
        public DateTimeOffset? ExecutionStarted { get; set; }
        public DateTimeOffset? ExecutionCompleted { get; set; }
        public DateTimeOffset? ExecutionCancelled { get; set; }

        public DateTimeOffset? TimerAdded { get; set; }
        public DateTimeOffset? TimerExpired { get; set; }

        public string ExecutionStatus { get; set; }
        public string ExecutionError { get; set; }
    }
}