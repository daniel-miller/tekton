using System.Collections.Generic;

namespace Atom.Common
{
    public class ValidationFailure
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}