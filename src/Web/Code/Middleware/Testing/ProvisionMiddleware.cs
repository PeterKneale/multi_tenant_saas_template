using Core.Application.Auth.Commands;
using Core.Application.Users.Commands;
using Newtonsoft.Json.Linq;

namespace Web.Code.Middleware.Testing;

public class ProvisionMiddleware(IMediator mediator, ILogger<ProvisionMiddleware> logs) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/test/provision") && context.Request.Method.Equals("POST"))
        {
            logs.LogWarning("🟠 Provisioning a test organisation and user");

            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var json = JObject.Parse(body);
            logs.LogInformation($"🟠 Provisioning with {json}");

            var organisationId = GetOrganisationId(json);
            var organisationName = Guid.NewGuid().ToString();
            var userId = GetUserId(json);
            var email = GetEmail(json);
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var password = GetPassword(json);

            logs.LogInformation($"🟠 OrganisationId:{organisationId}");
            logs.LogInformation($"🟠 UserId:{userId}");

            await mediator.Send(new Register.Command(organisationId, organisationName, userId, firstName, lastName,
                email, password));
            await mediator.Send(new VerifyEmailAddressWithoutToken.Command(email));
            context.Response.StatusCode = StatusCodes.Status200OK;
            return;
        }

        await next(context);
    }

    private static Guid GetOrganisationId(JObject context)
    {
        return GetGuid(context, "OrganisationId", Guid.NewGuid());
    }

    private static Guid GetUserId(JObject context)
    {
        return GetGuid(context, "UserId", Guid.NewGuid());
    }

    private static string GetEmail(JObject context)
    {
        return GetString(context, "Email", $"{Guid.NewGuid()}@example.com");
    }

    private static string GetPassword(JObject context)
    {
        return GetString(context, "Password", Guid.NewGuid().ToString());
    }

    private static Guid GetGuid(JObject context, string key, Guid defaultValue)
    {
        return context.TryGetValue(key, StringComparison.OrdinalIgnoreCase, out var token)
            ? Guid.Parse(token.ToString())
            : defaultValue;
    }

    private static string GetString(JObject context, string key, string defaultValue)
    {
        return context.TryGetValue(key, StringComparison.OrdinalIgnoreCase, out var token)
            ? token.ToString()
            : defaultValue;
    }
}