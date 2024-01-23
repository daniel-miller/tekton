using System.Collections.Generic;

namespace Common.Contract
{
    public class ValidationFailureResponse
    {
        public IEnumerable<ValidationResponse> Errors { get; set; }
    }
}