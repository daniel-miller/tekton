using System;

namespace Tek.Bus
{
    /// <summary>
    /// Provides the features for a basic service bus that supports sending and scheduling commands.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Registers a handler for a specific command.
        /// </summary>
        void Subscribe<T>(Action<T> action) where T : ICommand;

        /// <summary>
        /// Register a custom shard-specific handler for the command.
        /// </summary>
        void Override<T>(Action<T> action, int shard) where T : ICommand;

        /// <summary>
        /// Executes the command as a synchronous operation. 
        /// </summary>
        void Execute(ICommand command);

        /// <summary>
        /// Executes the command as an asynchronous operation, scheduled for execution at a specific date and time.
        /// </summary>
        void Schedule(ICommand command, DateTimeOffset? at = null);

        /// <summary>
        /// Wakes the queue to check for pending scheduled commands.
        /// </summary>
        void Ping(Action<int> scheduledCommandRead);

        /// <summary>
        /// Starts a scheduled command.
        /// </summary>
        bool Start(Guid command);

        /// <summary>
        /// Cancels a scheduled command.
        /// </summary>
        void Cancel(Guid command);

        /// <summary>
        /// Completes a scheduled command.
        /// </summary>
        void Complete(Guid command);

        /// <summary>
        /// Inserts a command for future reference. It is not executed or scheduled.
        /// </summary>
        void Bookmark(ICommand command, DateTimeOffset expired);
    }
}
