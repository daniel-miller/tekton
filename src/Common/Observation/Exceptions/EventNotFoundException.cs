using System;

namespace Common.Observation
{
    [Serializable]
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(string @class) 
            : base($"This event class is not found ({@class}).")
        {
        }
    }
}