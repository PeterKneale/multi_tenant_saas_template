using Npgsql;

namespace Core.Infrastructure.Database;

public static class DbHealthCheck
{
    public static void CheckDbAvailable(this IServiceProvider provider, IConfiguration configuration)
    {
        using var scope = provider.CreateScope();

        var logs = scope.ServiceProvider.GetRequiredService<ILogger<int>>();

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(1), (exception, timeSpan, attempt, context) =>
                logs.LogWarning(
                    "Attempt {Attempt} failed with exception {ExceptionMessage}. Delaying {TimeSpanTotalMilliseconds}ms",
                    attempt, exception.Message, timeSpan.TotalMilliseconds));

        policy.Execute(() =>
        {
            logs.LogInformation("Connecting to db...");
            using var connection = new NpgsqlConnection(configuration.GetDbConnectionString());
            using var command = new NpgsqlCommand("SELECT 1;", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            logs.LogInformation("Connected to db");
        });
    }
}