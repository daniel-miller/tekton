using System;

namespace Atom.Common
{
    public class Role : Model
    {
        public RoleType ParseType()
        {
            if (Enum.TryParse(Type, out RoleType type))
                return type;

            return RoleType.None;
        }
    }
}