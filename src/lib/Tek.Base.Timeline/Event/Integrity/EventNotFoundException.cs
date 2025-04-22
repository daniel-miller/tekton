using System;

namespace Tek.Base.Timeline
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