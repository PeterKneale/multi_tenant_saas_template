using Web.Code.Authentication;

namespace Web.Code.Middleware.Testing;

public class ImpersonateMiddleware(UserAuthService auth) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/test/Impersonate") && context.Request.Method.Equals("GET"))
        {
            var email = context.Request.Query["email"];
            var result = await auth.Impersonate(email);

            context.Response.StatusCode = result.IsSuccessful
                ? StatusCodes.Status200OK
                : StatusCodes.Status401Unauthorized;

            return; // End the request here.
        }

        // Call the next delegate/middleware in the pipeline
        await next(context);
    }
}