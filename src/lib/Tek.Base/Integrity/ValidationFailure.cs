using System.Collections.Generic;

namespace Tek.Base
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}