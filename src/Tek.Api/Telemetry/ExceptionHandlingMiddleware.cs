namespace Microsoft.Extensions.DependencyInjection;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMonitor _monitor;

    public ExceptionHandlingMiddleware(RequestDelegate next, IMonitor monitor)
    {
        _next = next;
        _monitor = monitor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _monitor.Error(ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new Error
            {
                Code = 500,
                Description = ex.StackTrace?.ToString(),
                Summary = ex.Message
            };

            var json = System.Text.Json.JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
