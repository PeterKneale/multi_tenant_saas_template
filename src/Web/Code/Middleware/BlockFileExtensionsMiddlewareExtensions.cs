namespace Web.Code.Middleware;

public static class BlockFileExtensionsMiddlewareExtensions
{
    public static IApplicationBuilder UseBlockFileExtensions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockFileExtensionsMiddleware>();
    }
}