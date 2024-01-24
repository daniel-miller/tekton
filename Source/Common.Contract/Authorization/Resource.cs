using System;

namespace Common.Contract.Security
{
    public class Resource : Model
    {
        public ResourceLocation Location { get; set; }

        public ResourceCategory ParseCategory()
        {
            if (Enum.TryParse(Category, out ResourceCategory category))
                return category;

            return ResourceCategory.None;
        }
    }
}
