using API.Attributes;

namespace API.Middlewares
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogExceptionAsync(context, ex);
                throw;
            }
        }

        private async Task LogExceptionAsync(HttpContext context, Exception ex)
        {
            Endpoint? endpoint = context.GetEndpoint();
            LogOnErrorAttribute? logErrorAttribute = endpoint?.Metadata.GetMetadata<LogOnErrorAttribute>();
            string logMessage = logErrorAttribute?.ErrorMessage ?? "An unexpected error occurred.";

            _logger.LogError(ex, logMessage);
        }
    }

}
