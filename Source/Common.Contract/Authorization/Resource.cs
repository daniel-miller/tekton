using System;

namespace Common.Contract
{
    public class Resource : Model
    {
        public ResourceLocation Location { get; set; }

        public ResourceClassification ParseClassification()
        {
            if (Enum.TryParse(Classification, out ResourceClassification classification))
                return classification;

            return ResourceClassification.None;
        }
    }
}
