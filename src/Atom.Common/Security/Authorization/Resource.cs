using System;

namespace Atom.Common
{
    public class Resource : Model
    {
        public ResourceLocation Location { get; set; }

        public ResourceType ParseType()
        {
            if (Enum.TryParse(Type, out ResourceType type))
                return type;

            return ResourceType.None;
        }
    }
}