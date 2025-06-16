namespace Web.Code.Middleware;

public class BlockFileExtensionsMiddleware(RequestDelegate next, ILogger<BlockFileExtensionsMiddleware>? logger = null)
{
    private readonly HashSet<string> _blockedPrefix =
    [
        "/wp", "/cgi-bin", "/.git", "/.well-known"
    ];

    private readonly HashSet<string> _blockedSuffix =
    [
        ".php", "ads.txt", ".json", ".env", "sitemap.xml"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request path has a blocked extension
        var requestPath = context.Request.Path.ToString();
        var isPrefixBlocked =
            _blockedPrefix.Any(ext => requestPath.StartsWith(ext, StringComparison.OrdinalIgnoreCase));
        var isSuffixBlocked = _blockedSuffix.Any(ext => requestPath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        if (isPrefixBlocked || isSuffixBlocked)
        {
            logger?.LogWarning("Blocked request to {Path}.", requestPath);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Not found");
            return;
        }

        // Continue to the next middleware
        await next(context);
    }
}