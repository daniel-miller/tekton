﻿using System;
using System.Runtime.Serialization;

namespace Tek.Bus
{
    [Serializable]
    public class AmbiguousCommandHandlerException : Exception
    {
        public AmbiguousCommandHandlerException(string name)
            : base($"You cannot define multiple handlers for the same command ({name}).")
        {
        }

        protected AmbiguousCommandHandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}