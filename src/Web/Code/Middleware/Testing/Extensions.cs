namespace Web.Code.Middleware.Testing;

public static class Extensions
{
    public static IServiceCollection AddTestingMiddleware(this IServiceCollection services)
    {
        return services
            .AddScoped<GetUserIdMiddleware>()
            .AddScoped<GetUserVerificationMiddleware>()
            .AddScoped<GetUserForgotPasswordTokenMiddleware>()
            .AddScoped<GetUserInvitationIdMiddleware>()
            .AddScoped<ImpersonateMiddleware>()
            .AddScoped<ProvisionMiddleware>();
    }

    public static IApplicationBuilder UseTestingMiddleware(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<GetUserIdMiddleware>()
            .UseMiddleware<GetUserVerificationMiddleware>()
            .UseMiddleware<GetUserForgotPasswordTokenMiddleware>()
            .UseMiddleware<GetUserInvitationIdMiddleware>()
            .UseMiddleware<ImpersonateMiddleware>()
            .UseMiddleware<ProvisionMiddleware>();
    }
}