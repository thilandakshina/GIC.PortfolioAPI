using System.Net;
using System.Text.Json;

namespace PortfolioAPI.WebApi.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message, type) = exception switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message, "BusinessRuleError"),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message, "ValidationError"),
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message, "NotFoundError"),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred", "SystemError")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                type = type,
                message = message,
                details = new { },
                timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
} 