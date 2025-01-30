using System;

namespace Tek.Contract
{
    public class Role : Model
    {
        public Role() { }

        public Role(Guid id)
        {
            Identifier = id;
        }

        public Role(Guid id, string name)
            : this(id)
        {
            Name = name;
        }
    }
}