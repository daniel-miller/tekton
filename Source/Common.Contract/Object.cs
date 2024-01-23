using System;

namespace Common.Contract
{
    public class Object
    {
        public Guid Identifier { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}