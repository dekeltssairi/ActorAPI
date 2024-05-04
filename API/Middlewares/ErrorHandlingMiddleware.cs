using API.Attributes;
using Domain.Exceptions;
using System.Text.Json;

namespace API.Middlewares
{

    public class ExceptionResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionResponseMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            var apiError = new ApiError();

            switch (ex)
            {
                case NotFoundException:
                    apiError.StatusCode = response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case ConflictException:
                    apiError.StatusCode = response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                default:
                    apiError.StatusCode = response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            apiError.Message = ex.Message;

            if (_env.IsDevelopment())
            {
                apiError.Details = ex.StackTrace;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await response.WriteAsync(JsonSerializer.Serialize(apiError, options));
        }
    }


}
