using System.Security.Claims;
using Core.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Code.Configuration;

namespace Web.Code.Authentication;

public class AdminAuthService(IHttpContextAccessor accessor, SiteOptions options, ILogger<AdminAuthService> logs)
{
    public async Task<AuthResult> Authenticate(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(options.AdminEmail) || string.IsNullOrWhiteSpace(options.AdminPassword))
        {
            logs.LogWarning("🔒 Admin authentication is not configured. Please set AdminEmail and AdminPassword in the configuration.");
            return AuthResult.Failure("🔒 Admin authentication is not configured");
        }

        var correctEmail = email.Equals(options.AdminEmail, StringComparison.InvariantCultureIgnoreCase);
        var correctPassword = password.Equals(options.AdminPassword, StringComparison.InvariantCulture);
        if (!correctEmail || !correctPassword)
        {
            return AuthResult.Failure("🔒 Admin login failed");
        }
        
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
}