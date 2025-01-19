using System.Collections.Generic;

namespace Tek.Common
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}