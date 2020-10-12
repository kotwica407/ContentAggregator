using System;
using System.Net;

namespace ContentAggregator.Models.Exceptions
{
    public class HttpErrorException : Exception
    {
        public HttpErrorException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
        public HttpStatusCode HttpStatusCode { get; }
    }
}