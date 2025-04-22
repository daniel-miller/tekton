﻿using System;
using System.Runtime.Serialization;

namespace Tek.Base.Timeline
{
    [Serializable]
    internal class SnapshotSerializationFailedException : Exception
    {
        public SnapshotSerializationFailedException()
        {
        }

        public SnapshotSerializationFailedException(string message) : base(message)
        {
        }

        public SnapshotSerializationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SnapshotSerializationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}