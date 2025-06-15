using System.Security.Claims;

namespace Web.Code.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserIdClaim(this ClaimsPrincipal principal)
    {
        return Guid.Parse(principal.FindFirstValue("UserId"));
    }

    public static Guid GetOrganisationIdClaim(this ClaimsPrincipal principal)
    {
        return Guid.Parse(principal.FindFirstValue("OrganisationId"));
    }
}