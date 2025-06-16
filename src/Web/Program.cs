using Core.Application;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Infrastructure;
using Core.Infrastructure.Database;
using Core.Infrastructure.Database.Migrations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Web.Code;
using Web.Code.Authentication;
using Web.Code.Middleware;
using Web.Code.Middleware.Testing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        // Home
        options.Conventions.AllowAnonymousToPage("/Index");
        // Auth
        options.Conventions.AllowAnonymousToPage("/Auth/Accept");
        options.Conventions.AllowAnonymousToPage("/Auth/Forbidden");
        options.Conventions.AllowAnonymousToPage("/Auth/Forgot");
        options.Conventions.AllowAnonymousToPage("/Auth/Login");
        options.Conventions.AllowAnonymousToPage("/Auth/Logout");
        options.Conventions.AllowAnonymousToPage("/Auth/Register");
        options.Conventions.AllowAnonymousToPage("/Auth/Registered");
        options.Conventions.AllowAnonymousToPage("/Auth/Reset");
        options.Conventions.AllowAnonymousToPage("/Auth/Verify");
        options.Conventions.AllowAnonymousToPage("/Error");
        options.Conventions.AllowAnonymousToPage("/Help");
        options.Conventions.AuthorizeFolder("/Admin", RoleConstants.IsAdmin);
        options.Conventions.AuthorizeFolder("/Manage", RoleConstants.IsTenant);
    })
    .AddRazorRuntimeCompilation()
    .AddDataAnnotationsLocalization();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Forbidden";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleConstants.IsAdmin, policy => policy.RequireRole(UserRole.AdminRoleName));
    options.AddPolicy(RoleConstants.IsTenant,
        policy => policy.RequireRole(UserRole.MemberRoleName, UserRole.OwnerRoleName));
});

builder.Services
    .AddScoped<ICurrentContext, CurrentContext>()
    .AddScoped<AdminAuthService>()
    .AddScoped<UserAuthService>()
    .AddHttpContextAccessor();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetDbConnectionString(), tags: new[] { "db" });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

if (builder.Environment.IsDevelopment()) builder.Services.AddTestingMiddleware();

var app = builder.Build();

var assembly = typeof(Program).Assembly.GetName();
var applicationName = $"{assembly.Name} {assembly.Version}";
var log = app.Services.GetRequiredService<ILogger<int>>();
log.LogInformation($"🚦Starting {applicationName} in {builder.Environment.EnvironmentName}");

app.UseExceptionHandler("/Error");
app.UseHsts();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GetRobotsTxtMiddleware>();
app.UseMiddleware<SetContextMiddleware>();
app.UseMiddleware<SetApiContextMiddleware>();
if (builder.Environment.IsDevelopment())
{
    log.LogInformation("⚠️ Using development middleware ⚠️");
    app.UseTestingMiddleware();
}

app.MapStaticAssets();
app.MapRazorPages().RequireAuthorization().WithStaticAssets();
app.MapHealthChecks("/health/alive", new HealthCheckOptions
{
    Predicate = _ => true
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true
});

log.LogInformation("🚦Checking database is available");
app.Services.CheckDbAvailable(builder.Configuration);

log.LogInformation("🚦Applying database migrations");
app.Services.ApplyDatabaseMigrations();

log.LogInformation($"🚦Started {applicationName} in {builder.Environment.EnvironmentName}");

app.Run();