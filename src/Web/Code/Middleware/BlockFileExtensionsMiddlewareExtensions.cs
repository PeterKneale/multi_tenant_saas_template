namespace Web.Core.Middleware;

public static class BlockFileExtensionsMiddlewareExtensions
{
    public static IApplicationBuilder UseBlockFileExtensions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockFileExtensionsMiddleware>();
    }
}