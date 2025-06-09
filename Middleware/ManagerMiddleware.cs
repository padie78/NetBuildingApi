using System.Net;

namespace NetBuilding.Middleware;

public class ManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManagerMiddleware> _logger;

    public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation($"Request Path: {context.Request.Path}");
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManagerExceptionAsync(context, ex, _logger);
        }
    }

    private async Task ManagerExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        object? errors = null;
        switch (ex)
        {
            case MiddlewareException middlewareException:
                logger.LogError(ex, "Middleware Exception: Error Middleware");
                context.Response.StatusCode = (int)middlewareException.StatusCode;
                errors = middlewareException.Errors;
                break;
            case Exception e:
                logger.LogError(ex, "Exception: Error Server");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errors = string.IsNullOrWhiteSpace(e.Message) ? "An error occurred" : e.Message;
                break;
        }
        context.Response.ContentType = "application/json"; 
        if (errors != null)
        {
           var results = System.Text.Json.JsonSerializer.Serialize(new{errors});
           await context.Response.WriteAsync(results);
        }       
    }
}

