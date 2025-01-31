using System.Collections.Generic;

namespace Tek.Contract
{
    public class SentinelsSettings
    {
        public Sentinel React { get; set; }
        public Sentinel Root { get; set; }
        public Sentinel Someone { get; set; }
        public Sentinel Test { get; set; }

        public Sentinel[] ToArray()
        {
            var list = new List<Sentinel>();
            
            if (React != null)
                list.Add(React);

            if (Root != null)
                list.Add(Root);

            if (Someone != null)
                list.Add(Someone);

            if (Test != null)
                list.Add(Test);

            return list.ToArray();
        }
    }

    public class Sentinel : Actor
    {
        public string[] Roles { get; set; }
    }
}
