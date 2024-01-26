using System;

namespace Common.Contract
{
    public class Function : Model
    {
        public FunctionClassification ParseClassification()
        {
            if (Enum.TryParse(Classification, out FunctionClassification classification))
                return classification;

            return FunctionClassification.None;
        }
    }
}
