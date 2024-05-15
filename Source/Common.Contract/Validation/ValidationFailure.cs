using System.Collections.Generic;

namespace Common.Contract
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}