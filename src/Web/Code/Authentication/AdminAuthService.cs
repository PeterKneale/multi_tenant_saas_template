using System.Security.Claims;
using Core.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Web.Code.Configuration;

namespace Web.Code.Authentication;

public class AdminAuthService(IHttpContextAccessor accessor, IOptions<SiteOptions> site, ILogger<AdminAuthService> logs)
{
    public async Task<AuthResult> Authenticate(string email, string password)
    {
        var options = site.Value ?? throw new InvalidOperationException("Site options not found");

        var correctEmail = email.Equals(options.AdminEmail, StringComparison.InvariantCultureIgnoreCase);
        var correctPassword = password.Equals(options.AdminPassword, StringComparison.InvariantCulture);
        if (correctEmail && correctPassword)
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, "Admin"),
                new(ClaimTypes.Email, options.AdminEmail),
                new(ClaimTypes.Role, UserRole.AdminRoleName)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            logs.LogInformation("🔓 Site Admin Authentication was successful: {Email}", email);
            return AuthResult.Success();
        }

        return AuthResult.Failure("🔒 Admin login failed");
    }
}