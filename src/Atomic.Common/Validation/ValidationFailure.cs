using System.Collections.Generic;

namespace Atomic.Common
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}