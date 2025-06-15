namespace Core.Infrastructure.Database;

public static class DbConfigurationExtensions
{
    private const string Template = "Username={0};Password={1};Database={2};Host={3};Port={4};";

    public static string GetDbConnectionString(this IConfiguration configuration)
    {
        var username = configuration["DB_USERNAME"] ?? "postgres";
        var password = configuration["DB_PASSWORD"] ?? "password";
        var database = configuration["DB_DATABASE"] ?? "db";
        var host = configuration["DB_HOST"] ?? "localhost";
        var port = configuration["DB_PORT"] ?? "5432";
        return string.Format(Template, username, password, database, host, port);
    }
}