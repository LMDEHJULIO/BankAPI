using BankAPI.Models;
using System.Net;

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

                await context.Response.WriteAsJsonAsync(new APIResponse(HttpStatusCode.InternalServerError, null, "An unexpected error occurred");
            }   
        }

    }
}
