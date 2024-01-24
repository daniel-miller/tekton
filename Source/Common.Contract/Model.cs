using System;

namespace Common.Contract
{
    public class Model
    {
        public Guid Identifier { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}