
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Pobierz klucz API z nagłówka
        if (!httpContext.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("API Key is missing");
            return;
        }

        // Sprawdź, czy klucz API jest prawidłowy
        var validApiKey = _configuration["ApiKey"];
        if (apiKey != validApiKey)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(httpContext); // Jeśli klucz jest poprawny, kontynuuj obsługę żądania
    }
}
