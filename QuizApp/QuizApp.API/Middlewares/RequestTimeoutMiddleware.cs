namespace QuizApp.API.Middlewares
{
    public class RequestTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TimeSpan _timespan;

        public RequestTimeoutMiddleware(RequestDelegate next,IConfiguration configuration)
        {
            _next = next;
            _timespan = TimeSpan.FromMilliseconds(int.Parse(configuration?["Timeout:Apis"] ?? string.Empty));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var cts = new CancellationTokenSource(_timespan);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token,context.RequestAborted);

            try
            {
                context.RequestAborted = linkedCts.Token;
                await _next(context);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                await context.Response.WriteAsync("Request Timed out");
            }
        }
    }
}
