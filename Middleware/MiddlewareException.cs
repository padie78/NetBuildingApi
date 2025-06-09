using System.Net;

namespace NetBuilding.Middleware;

public class MiddlewareException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public object? Errors { get; set; }
    public MiddlewareException(HttpStatusCode StatusCode, object? Errors = null)
    {
        this.StatusCode = StatusCode;
        this.Errors = Errors;
    }
}