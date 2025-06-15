using Core.Application.Contracts;
using Core.Domain.Common;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;

namespace Web.Code.Middleware.Testing;

public class GetUserInvitationIdMiddleware(IReadOnlyRepository<Invitation> invitationRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/test/GetUserInvitationId") &&
            context.Request.Method.Equals("GET"))
        {
            var email = EmailAddress.Create(context.Request.Query["Email"]);
            var invitation = await invitationRepository.SingleOrDefaultAsync(new GetInvitationByEmailGlobally(email));
            if (invitation is null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Invitation not found.");
                return; // End the request here.
            }

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(invitation.Id.Value.ToString());
            return; // End the request here.
        }

        // Call the next delegate/middleware in the pipeline
        await next(context);
    }
}