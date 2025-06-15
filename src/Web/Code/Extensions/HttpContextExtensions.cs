using System.Security.Claims;
using Core.Domain.Organisations;
using Core.Domain.Users;

namespace Web.Code.Extensions;

public static class HttpContextExtensions
{
    private const string OrganisationKey = "Organisation";
    private const string UserKey = "User";

    public static bool IsCurrentOrganisationSet(this HttpContext context)
    {
        return context.Items.ContainsKey(OrganisationKey);
    }

    public static void SetCurrentOrganisation(this HttpContext context, Organisation organisation)
    {
        context.Items.Add(OrganisationKey, organisation);
    }

    public static void SetCurrentUser(this HttpContext context, User user)
    {
        context.Items.Add(UserKey, user);
    }

    public static Organisation GetCurrentOrganisation(this HttpContext context)
    {
        return context.Items[OrganisationKey] as Organisation
               ?? throw new InvalidOperationException("No organisation context is available in http context");
    }

    public static bool IsCurrentUserSet(this HttpContext context)
    {
        return context.Items.ContainsKey(UserKey);
    }

    public static User GetCurrentUser(this HttpContext context)
    {
        return context.Items[UserKey] as User
               ?? throw new InvalidOperationException("No user context is available in http context");
    }

    public static bool IsImpersonating(this HttpContext context)
    {
        return context.User.HasClaim(x => x.Type == ClaimTypes.System);
    }
}