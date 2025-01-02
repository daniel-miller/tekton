namespace Atomic.Common
{
    /// <summary>
    /// An actor represents an individual person or group or system that performs actions through the UI and/or API.
    /// </summary>
    public class Actor : Model
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Secret { get; set; }
    }
}