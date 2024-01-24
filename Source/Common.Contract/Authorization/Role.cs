using System;

namespace Common.Contract.Security
{
    public class Role : Model
    {
        public RoleCategory ParseCategory()
        {
            if (Enum.TryParse(Category, out RoleCategory category))
                return category;

            return RoleCategory.None;
        }
    }
}
