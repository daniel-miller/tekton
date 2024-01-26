using System;

namespace Common.Timeline.Changes
{
    [Serializable]
    internal class ChangeNotFoundException : Exception
    {
        public ChangeNotFoundException(string @class) 
            : base($"This change class is not found ({@class}).")
        {
        }
    }
}