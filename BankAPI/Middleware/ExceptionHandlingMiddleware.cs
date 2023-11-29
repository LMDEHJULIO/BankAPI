using BankAPI.Exceptions;
using BankAPI.Models;
using System.Net;
using System.Text.Json;

namespace BankAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new APIResponse(HttpStatusCode.InternalServerError, null, "An unexpected error occurred"));
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception) {

            var errorDetail = new ErrorDetail
            {
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Title = exception.Message,
                DeveloperMessage = exception.Message,
            };

            switch (exception) {
                case ResourceNotFoundException _:
                    errorDetail.Status = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    errorDetail.Status = (int)(HttpStatusCode.InternalServerError);
                    break;

            }

            var result = JsonSerializer.Serialize(errorDetail);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetail.Status;
            return context.Response.WriteAsync(result);
        }

    }
}
