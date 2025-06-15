using Core.Application.Contracts;
using Core.Domain.Common;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Web.Core.Middleware;

public class SetApiContextMiddleware(RequestDelegate next)
{
    private const string ApiPath = "/api";
    private const string ApiKeyHeader = "X-APIKEY";

    public async Task InvokeAsync(HttpContext context, ILogger<int> log,
        IReadOnlyRepository<Organisation> organisations, IReadOnlyRepository<User> users)
    {
        log.LogDebug("Api context middleware");

        if (!context.Request.Path.StartsWithSegments(new PathString(ApiPath)))
        {
            log.LogDebug($"Request ignored, does not start with '{ApiPath}'");
            await next(context);
            return;
        }

        log.LogDebug("Detected API Request Path: {path}", context.Request.Path);

        if (!context.Request.Headers.ContainsKey(ApiKeyHeader))
        {
            log.LogWarning($"Request denied, no '{ApiKeyHeader}' header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var apiKeyHeader = context.Request.Headers[ApiKeyHeader];
        if (string.IsNullOrWhiteSpace(apiKeyHeader))
        {
            log.LogWarning($"Request denied, empty '{ApiKeyHeader}' header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        log.LogDebug("API request with api key header {apikey}", apiKeyHeader);
        if (!Guid.TryParse(apiKeyHeader, out var apiKey))
        {
            log.LogWarning($"Request denied, invalid '{ApiKeyHeader}' header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // HACK
        // There are no specific api keys so for now we are just using the user id as the api key
        var userId = UserId.Create(apiKey);

        var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId));
        if (user == null)
        {
            log.LogWarning($"Request denied, user not found for userId '{userId}'");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var organisation =
            await organisations.SingleOrDefaultAsync(new GetOrganisationByIdGlobally(user.OrganisationId));
        if (organisation == null)
        {
            log.LogWarning($"Request denied, organisation not found for user '{user.Id}'");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.SetCurrentOrganisation(organisation);
        context.SetCurrentUser(user);
        await next(context);
    }
}