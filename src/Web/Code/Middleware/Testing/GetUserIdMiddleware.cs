using Core.Application.Contracts;
using Core.Domain.Common;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Web.Code.Middleware.Testing;

public class GetUserIdMiddleware(IReadOnlyRepository<User> users) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/test/GetUserId") && context.Request.Method.Equals("GET"))
        {
            var email = EmailAddress.Create(context.Request.Query["Email"]);

            var user = await users.SingleOrDefaultAsync(new GetUserByEmailGlobalSpecification(email));
            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("User not found");
                return; // End the request here.
            }

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(user.Id.Value.ToString());
            return; // End the request here.
        }

        await next(context);
    }
}