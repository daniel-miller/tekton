﻿using System;

namespace Tek.Base.Timeline
{
    /// <summary>
    /// Defines the minimum set of properties that every command must implement.
    /// </summary>
    /// <remarks>
    /// A command is a request to change the domain. It is always are named with a verb in the imperative mood, such as 
    /// Confirm Order. Unlike an event, a command is not a statement of fact; it is only a request, and thus may be 
    /// refused. Commands are immutable because their expected usage is to be sent directly to the domain model for 
    /// processing. They do not need to change during their projected lifetime.
    /// </remarks>
    public interface ICommand
    {
        Guid AggregateIdentifier { get; set; }
        int? ExpectedVersion { get; set; }

        int OriginShard { get; set; }
        Guid OriginActor { get; set; }

        Guid CommandIdentifier { get; set; }
    }
}
