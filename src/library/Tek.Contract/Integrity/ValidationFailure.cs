using System.Collections.Generic;

namespace Tek.Contract
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}