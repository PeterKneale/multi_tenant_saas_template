using System.Security.Claims;
using Core.Application.Auth.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Code.Authentication;

public class UserAuthService(IHttpContextAccessor accessor, ISender mediator, ILogger<AdminAuthService> logs)
{
    public async Task<AuthResult> Impersonate(string email)
    {
        try
        {
            var user = await mediator.Send(new CanImpersonate.Query(email));
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.System, "Impersonated"),
                new("UserId", user.UserId.ToString()),
                new("OrganisationId", user.OrganisationId.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme));
            await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            logs.LogInformation("🔓 Impersonation was successful: {email}", email);
            return AuthResult.Success();
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("🔒 Impersonation was not successful: {Message}", e.Message);
            return AuthResult.Failure(e.Message);
        }
    }

    public async Task<AuthResult> Authenticate(string email, string password)
    {
        try
        {
            var result = await mediator.Send(new CanAuthenticate.Command(email, password));
            if (!result.IsSuccess)
            {
                logs.LogWarning("🔒 Authentication was not successful");
                return AuthResult.Failure("Authentication was not successful");
            }

            var user = result.Value;
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
                new("UserId", user.UserId.ToString()),
                new("OrganisationId", user.OrganisationId.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            logs.LogInformation("🔓 Authentication was successful: {Email}", email);
            return AuthResult.Success();
        }
        catch (BusinessRuleBrokenException e)
        {
            logs.LogWarning("🔒 Authentication was not successful: {Message}", e.Message);
            return AuthResult.Failure(e.Message);
        }
    }
}