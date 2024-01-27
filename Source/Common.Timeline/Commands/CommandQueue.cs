using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Common.Timeline.Exceptions;
using Common.Timeline.Services;

namespace Common.Timeline.Commands
{
    /// <summary>
    /// Implements a basic command queue. The purpose of the queue is to route commands to subscriber methods; 
    /// validation of a command itself is the responsibility of its subscriber/handler.
    /// </summary>
    public class CommandQueue : ICommandQueue
    {
        private readonly IJsonSerializer _serializer;

        /// <summary>
        /// A command's full class name is used as the key to find the method that handles it.
        /// </summary>
        readonly Dictionary<string, Action<ICommand>> _subscribers;

        /// <summary>
        /// In a multi-organization system we need to allow an individual organization to override/customize the handling of a 
        /// command. In this case the class name and the organization identifier are used together as the unique key.
        /// </summary>
        readonly Dictionary<(string, Guid), Action<ICommand>> _overriders;

        /// <summary>
        /// Scheduled commands must be stored. Unscheduled commands can be stored, but this is optional.
        /// </summary>
        readonly ICommandStore _store;

        /// <summary>
        /// True if all commands (scheduled and unscheduled) are logged to the command store. False if only scheduled 
        /// commands are logged.
        /// </summary>
        readonly bool _saveAll;

        /// <summary>
        /// Constructs the queue.
        /// </summary>
        public CommandQueue(ICommandStore store, bool saveAll = false)
        {
            _subscribers = new Dictionary<string, Action<ICommand>>();
            _overriders = new Dictionary<(string, Guid), Action<ICommand>>();
            _store = store;
            _saveAll = saveAll;

            _serializer = ServiceLocator.Instance.GetService<IJsonSerializer>();
        }

        #region Methods (subscription)

        /// <summary>
        /// One and only one subscriber can register for each command. If a command is sent then it must have a handler.
        /// </summary>
        public void Subscribe<T>(Action<T> action) where T : ICommand
        {
            var name = _serializer.GetClassName(typeof(T));

            if (_subscribers.Any(x => x.Key == name))
                throw new AmbiguousCommandHandlerException(name);

            _subscribers.Add(name, (command) => action((T)command));
        }

        /// <summary>
        /// Registers a organization-specific custom handler for the command.
        /// </summary>
        public void Override<T>(Action<T> action, Guid organization) where T : ICommand
        {
            var name = _serializer.GetClassName(typeof(T));

            if (_overriders.Any(x => x.Key.Item1 == name && x.Key.Item2 == organization))
                throw new AmbiguousCommandHandlerException(name);

            _overriders.Add((name, organization), (command) => action((T)command));
        }

        #endregion

        #region Methods (sending synchronous commands)

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        public void Send(ICommand command)
        {
            SerializedCommand serialized = null;

            if (_saveAll)
            {
                serialized = _store.Serialize(command);
                serialized.SendStarted = DateTimeOffset.UtcNow;
            }

            Execute(command, _serializer.GetClassName(command.GetType()));

            if (_saveAll)
            {
                serialized.SendCompleted = DateTimeOffset.UtcNow;
                serialized.SendStatus = "Completed";
                _store.Insert(serialized);
            }
        }

        #endregion

        #region Methods (scheduling asynchronous commands)

        /// <summary>
        /// Schedules the command for asynchronous execution.
        /// </summary>
        public void Schedule(ICommand command, DateTimeOffset? at = null)
        {
            var serialized = _store.Serialize(command);
            serialized.SendScheduled = at ?? DateTimeOffset.UtcNow;
            serialized.SendStatus = "Scheduled";
            _store.Insert(serialized);
        }

        /// <summary>
        /// Wakes the command queue to check for pending scheduled commands. Executes all commands for which the timer
        /// is now elapsed.
        /// </summary>
        public void Ping(Action<int> scheduledCommandRead)
        {
            var commands = _store.GetExpired(DateTimeOffset.UtcNow);
            scheduledCommandRead?.Invoke(commands.Length);

            foreach (var command in commands)
                Execute(command);
        }

        /// <summary>
        /// Forces a scheduled command to start.
        /// </summary>
        public bool Start(Guid command)
        {
            if (!_store.Exists(command))
                return false;

            Execute(_store.Get(command));
            return true;
        }

        /// <summary>
        /// Cancels a scheduled command.
        /// </summary>
        public void Cancel(Guid command)
        {
            var serialized = _store.Get(command);
            serialized.SendCancelled = DateTimeOffset.UtcNow;
            serialized.SendStatus = "Cancelled";
            _store.Update(serialized);
        }

        /// <summary>
        /// Completes a scheduled command.
        /// </summary>
        public void Complete(Guid command)
        {
            var serialized = _store.Get(command);
            serialized.SendCompleted = DateTimeOffset.UtcNow;
            serialized.SendStatus = "Completed";
            _store.Update(serialized);
        }

        #endregion

        #region Methods (bookmarking commands)

        /// <summary>
        /// Bookmarks the command for future reference.
        /// </summary>
        public void Bookmark(ICommand command, DateTimeOffset expired)
        {
            var serialized = _store.Serialize(command);
            serialized.BookmarkAdded = DateTimeOffset.Now;
            serialized.BookmarkExpired = expired;
            serialized.SendStatus = "Bookmarked";
            _store.Insert(serialized);
        }

        #endregion

        #region Methods (execution)

        private static readonly Regex TypeFullNamePattern = new Regex("^(.+), (.+), (.+), (.+), (.+)$", RegexOptions.Compiled);

        /// <summary>
        /// Invokes the subscriber method registered to handle the command.
        /// </summary>
        private void Execute(ICommand command, string @class)
        {
            // If the class name is fully-qualified with a Version, Culture, and PublicKeyToken 
            // then strip the latter properties from the class name.

            var match = TypeFullNamePattern.Match(@class);
            if (match.Success)
                @class = $"{match.Groups[1].Value}, {match.Groups[2].Value}";

            if (_overriders.ContainsKey((@class, command.OriginOrganization)))
            {
                var customization = _overriders[(@class, command.OriginOrganization)];
                customization.Invoke(command);
            }
            else if (_subscribers.ContainsKey(@class))
            {
                ExecuteSubscriberAction(command, @class);
            }
            else
            {
                throw new UnhandledCommandException(@class);
            }
        }

        private void ExecuteSubscriberAction(ICommand command, string @class)
        {
            try
            {
                var action = _subscribers[@class];
                action.Invoke(command);
            }
            catch (Exception ex)
            {
                var message = $"An unexpected error occurred executing this command ({@class}) with AggregateIdentifier {command.AggregateIdentifier}. {ex.Message}";
                message += " -- Here are the properties of this command: " + SerializeCommandForError(command);

                throw new UnhandledCommandException(message, ex);
            }
        }

        private string SerializeCommandForError(ICommand command)
        {
            try
            {
                var serialized = _store.Serialize(command);
                return $"OriginOrganization = {serialized.OriginOrganization}"
                     + $", OriginUser = {serialized.OriginUser}"
                     + $", CommandData = {serialized.CommandData}";
            }
            catch
            {
                return "Command: Error during the command serialization.";
            }
        }

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        private void Execute(SerializedCommand serialized)
        {
            var now = DateTimeOffset.UtcNow;

            serialized.SendStarted = now;
            serialized.SendStatus = "Started";

            _store.Update(serialized);

            // Uncomment this code to implement recurring commands. We may never do this, because all our recurring
            // commands scheduled and managed in Octopus.

            // bool recur = serialized.RecurrenceInterval.HasValue && serialized.RecurrenceUnit != null;
            // bool skip = recur && !Shift.Utility.Calendar.WeekdaysContain(serialized.RecurrenceWeekdays, now.DayOfWeek);
            // if (!skip)
                Execute(serialized.Deserialize(false), serialized.CommandClass);

            serialized.SendCompleted = DateTimeOffset.UtcNow;
            serialized.SendStatus = "Completed";

            // Uncomment this code to implement recurring commands.
            /* 
            if (recur && serialized.SendScheduled.HasValue)
            {
                var next = Shift.Utility.Calendar.CalculateNextInterval(serialized.SendScheduled.Value, DateTimeOffset.UtcNow, serialized.RecurrenceUnit, serialized.RecurrenceInterval.Value);
                if (next > serialized.SendScheduled)
                {
                    serialized.SendScheduled = next;
                    serialized.SendStarted = null;
                    serialized.SendCompleted = null;
                    serialized.SendStatus = "Scheduled";
                }
            }
            */

            _store.Update(serialized);
        }

        #endregion
    }
}
