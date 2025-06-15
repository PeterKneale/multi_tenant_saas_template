using Core.Application.Contracts;
using Core.Domain.Common;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Core.Middleware;

public class SetContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<int> log,
        IReadOnlyRepository<Organisation> organisations, IReadOnlyRepository<User> users)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            if (context.User.IsInRole(UserRole.AdminRoleName))
            {
                // No context to set for site administrators
            }
            else
            {
                var organisationId = OrganisationId.Create(context.User.GetOrganisationIdClaim());
                var userId = UserId.Create(context.User.GetUserIdClaim());

                var organisation =
                    await organisations.SingleOrDefaultAsync(new GetOrganisationByIdGlobally(organisationId));
                var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId));

                if (organisation == null || user == null)
                {
                    log.LogWarning("Organisation or User not found, forcing logout");
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/");
                    return;
                }

                log.LogDebug("Setting context: User {User} and Organisation {Organisation}", user.Name.FullName,
                    organisation.Name.Title);

                var path = context.Request.Path;

                if (!context.IsCurrentOrganisationSet())
                    context.SetCurrentOrganisation(organisation);
                else
                    log.LogWarning("The current organisation was already set, this should not happen. Path {path}",
                        path);

                if (!context.IsCurrentUserSet())
                    context.SetCurrentUser(user);
                else
                    log.LogWarning("The current user was already set, this should not happen. Path {path}", path);
            }
        }

        await next(context);
    }
}