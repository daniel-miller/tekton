namespace Tek.Contract
{
    public class SentinelsSettings
    {
        public Sentinel React { get; set; }
        public Sentinel Root { get; set; }
        public Sentinel Someone { get; set; }
        public Sentinel Test { get; set; }

        public Sentinel[] ToArray()
            => new[] { React, Root, Someone, Test };
    }

    public class Sentinel : Actor
    {
        public string[] Roles { get; set; }
    }
}
