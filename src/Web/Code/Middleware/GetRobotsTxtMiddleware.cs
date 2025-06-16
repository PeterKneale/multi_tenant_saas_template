namespace Web.Code.Middleware;

public class GetRobotsTxtMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env)
    {
        if (context.Request.Path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase) &&
            context.Request.Method.Equals("GET"))
        {
            var content = env.IsProduction()
                ? "User-agent: *\nDisallow: " // allow everything
                : "User-agent: *\nDisallow: /"; // block everything
            await context.Response.WriteAsync(content);
            return; // End the request here.
        }

        // Call the next delegate/middleware in the pipeline
        await next(context);
    }
}