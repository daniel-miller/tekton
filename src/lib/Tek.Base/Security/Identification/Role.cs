﻿using System;

namespace Tek.Base
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