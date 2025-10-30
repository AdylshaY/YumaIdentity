namespace YumaIdentity.API.Middleware
{
    using System.Net;
    using System.Text.Json;
    using YumaIdentity.Application.Common.Exceptions;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            object errorResponse = new { Title = "", exception.Message };

            switch (exception)
            {
                case ValidationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new { Title = "Validation Error", ex.Message, ex.Errors };
                    break;

                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new { Title = "Not Found", ex.Message };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "An unhandled exception occurred.");
                    errorResponse = new { Title = "Internal Server Error", Message = "An unexpected error occurred. Please try again later." };
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
