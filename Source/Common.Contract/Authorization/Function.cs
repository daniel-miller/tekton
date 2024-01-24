using System;

namespace Common.Contract.Security
{
    public class Function : Model
    {
        public FunctionCategory ParseCategory()
        {
            if (Enum.TryParse(Category, out FunctionCategory category))
                return category;

            return FunctionCategory.None;
        }
    }
}
