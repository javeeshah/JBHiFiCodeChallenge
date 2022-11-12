namespace CurrentWeatherApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private
        const string APIKEY = "X-API-Key";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeys = appSettings.GetSection("API-Keys").Get<List<string>>();
            if (!apiKeys.Contains(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Provided Api key is invalid");
                return;
            }
            await _next(context);
        }
    }
}
