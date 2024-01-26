using System;

namespace Common.Contract
{
    public class Role : Model
    {
        public RoleClassification ParseClassification()
        {
            if (Enum.TryParse(Classification, out RoleClassification classification))
                return classification;

            return RoleClassification.None;
        }
    }
}
