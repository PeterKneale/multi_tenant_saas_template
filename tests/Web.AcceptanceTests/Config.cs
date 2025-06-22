using Microsoft.Extensions.Configuration;

namespace Web.AcceptanceTests;

public static class Config
{
    private static readonly IConfigurationRoot Configuration;

    static Config()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }

    public static Uri GetWebUri() =>
        new($"{GetWebScheme()}://{GetWebHost()}:{GetWebPort()}", UriKind.Absolute);

    public static Uri GetWebReadyUri() => 
        new(GetWebUri(), new Uri("/health/ready", UriKind.Relative));

    private static string GetWebScheme() =>
        GetString("WEB_SCHEME", "http");

    private static string GetWebHost() =>
        GetString("WEB_HOST", "localhost");
    
    private static string GetWebPort() =>
        GetString("WEB_PORT", "8080");

    public static bool GetHeadless() =>
        GetBoolean("HEADLESS", true);

    private static bool GetBoolean(string key, bool defaultValue) =>
        bool.Parse(GetString(key, defaultValue.ToString()));

    private static string GetString(string key, string defaultValue) =>
        Configuration[key] ?? defaultValue;

    public static string GetDefaultAdminEmail() => "admin@localhost.com";
    public static string GetDefaultAdminPassword() => "passwordpassword";
    public static string GetDefaultUserPassword() => "passwordpassword"; // >= 15
}