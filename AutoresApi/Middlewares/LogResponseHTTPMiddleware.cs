namespace AutoresApi.Middlewares
{
    public class LogResponseHTTPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseHTTPMiddleware> _logger;

        public LogResponseHTTPMiddleware(RequestDelegate next, ILogger<LogResponseHTTPMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream())
            {
                var originalBodyResponse = context.Response.Body;
                context.Response.Body = ms;

                await _next(context);

                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(originalBodyResponse);
                context.Response.Body = originalBodyResponse;

                _logger.LogInformation(response);
            }
        }
    }
}
