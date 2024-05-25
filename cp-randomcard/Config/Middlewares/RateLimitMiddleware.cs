namespace cp_randomcard.RateLimit
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static Dictionary<string, DateTime> _requestTracker;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
            _requestTracker = new Dictionary<string, DateTime>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress.ToString();
            if (context.Request.Path.ToString().Contains("/api/") && !context.Request.Path.ToString().Contains("health"))
            {
                if (_requestTracker.ContainsKey(clientIp) &&
                    DateTime.Now.Subtract(_requestTracker[clientIp]).TotalSeconds < 2)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Rate limit exceeded. Try again in 2 seconds.");
                    return;
                }
                _requestTracker[clientIp] = DateTime.Now;
            }

            await _next(context);
        }
    }
}
