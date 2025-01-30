using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace Tek.Contract
{
    public class ApiResult
    {
        public HttpStatusCode Status { get; set; }
        
        public List<Error> Errors { get; set; }

        public HttpResponseHeaders Headers { get; set; }

        public Pagination Pagination { get; set; }

        public ApiResult(HttpStatusCode status, HttpResponseHeaders headers)
        {
            Status = status;
            Errors = new List<Error>();
            Headers = headers;
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; } = default;

        public ApiResult(HttpStatusCode status, HttpResponseHeaders headers)
            : base(status, headers)
        {
            
        }
    }
}