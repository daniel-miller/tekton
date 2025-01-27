using System;

using Tek.Common;
using Tek.Contract;

namespace Tek.Bus
{
    /// <summary>
    /// Provides functions to convert between instances of ICommand and SerializedCommand.
    /// </summary>
    public class CommandSerializer
    {
        private readonly IJsonSerializer _serializer;

        public CommandSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Returns a deserialized command.
        /// </summary>
        public ICommand Deserialize(SerializedCommand x)
        {
            try
            {
                var data = _serializer.Deserialize<ICommand>(Type.GetType(x.CommandClass), x.CommandData, JsonPurpose.Storage);

                data.AggregateIdentifier = x.AggregateIdentifier;
                data.ExpectedVersion = x.ExpectedVersion;

                data.OriginShard = x.OriginShard;
                data.OriginActor = x.OriginActor;

                data.CommandIdentifier = x.CommandIdentifier;

                return data;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"{ex.Message} Command: Type = {x.CommandType}, Identifier = {x.CommandIdentifier}, Data = [{x.CommandData}]", ex);
            }
        }

        /// <summary>
        /// Returns a serialized command.
        /// </summary>
        public SerializedCommand Serialize(ICommand command)
        {
            var data = _serializer.Serialize(command, JsonPurpose.Storage, false, new[]
            {
                nameof(ICommand.AggregateIdentifier),
                nameof(ICommand.ExpectedVersion),
                nameof(ICommand.OriginShard),
                nameof(ICommand.OriginActor),
                nameof(ICommand.CommandIdentifier)
            });

            var reflector = new Reflector();

            var serialized = new SerializedCommand
            {
                AggregateIdentifier = command.AggregateIdentifier,
                ExpectedVersion = command.ExpectedVersion,

                CommandClass = reflector.GetClassName(command.GetType()),
                CommandType = command.GetType().Name,
                CommandData = data,

                CommandIdentifier = command.CommandIdentifier,

                OriginShard = command.OriginShard,
                OriginActor = command.OriginActor
            };

            if (serialized.CommandClass.Length > 200)
                throw new OverflowException($"The assembly-qualified name for this command ({serialized.CommandClass}) exceeds the maximum character limit (200).");

            if (serialized.CommandType.Length > 100)
                throw new OverflowException($"The type name for this command ({serialized.CommandType}) exceeds the maximum character limit (100).");

            if ((serialized.ExecutionStatus?.Length ?? 0) > 20)
                throw new OverflowException($"The execution status ({serialized.ExecutionStatus}) exceeds the maximum character limit (20).");

            return serialized;
        }
    }
}