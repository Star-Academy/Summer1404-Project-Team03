using etl_backend.Api.Exceptions;

namespace etl_backend.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var payload = new
            {
                error = ex.Message,
                details = ex.ResponseContent
            };
            
            await context.Response.WriteAsync(payload.ToString() ?? "");
        }
    }
}