using System.Net;

namespace ContentAggregator.Models.Exceptions
{
    public static class HttpError
    {
        public static HttpErrorException BadRequest(string message) =>
            new HttpErrorException(HttpStatusCode.BadRequest, message);

        public static HttpErrorException Unauthorized(string message) =>
            new HttpErrorException(HttpStatusCode.Unauthorized, message);

        public static HttpErrorException Forbidden(string message) =>
            new HttpErrorException(HttpStatusCode.Forbidden, message);

        public static HttpErrorException NotFound(string message) =>
            new HttpErrorException(HttpStatusCode.NotFound, message);

        public static HttpErrorException InternalServerError(string message) =>
            new HttpErrorException(HttpStatusCode.InternalServerError, message);
    }
}