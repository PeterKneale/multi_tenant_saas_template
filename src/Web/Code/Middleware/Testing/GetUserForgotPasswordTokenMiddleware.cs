using Core.Application.Contracts;
using Core.Domain.Common;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Web.Code.Middleware.Testing;

public class GetUserForgotPasswordTokenMiddleware(IReadOnlyRepository<User> users) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/test/GetUserForgotPasswordToken") &&
            context.Request.Method.Equals("GET"))
        {
            var email = EmailAddress.Create(context.Request.Query["Email"]);
            var specification = new GetUserByEmailGlobalSpecification(email);
            var user = await users.SingleOrDefaultAsync(specification);
            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("User not found");
                return; // End the request here.
            }

            if (user.ForgottenToken is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("User forgot password token not found");
                return; // End the request here.
            }

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(user.ForgottenToken);
            return; // End the request here.
        }

        // Call the next delegate/middleware in the pipeline
        await next(context);
    }
}