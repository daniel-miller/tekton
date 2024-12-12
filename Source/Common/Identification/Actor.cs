using System;

namespace Common
{
    /// <summary>
    /// An actor represents an individual person or group or system that performs actions through the UI and/or API.
    /// </summary>
    public class Actor
    {
        public Guid Identifier { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}