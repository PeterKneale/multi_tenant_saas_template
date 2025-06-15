namespace Core.Infrastructure.Database.Migrations;

public static class DbMigrationExtensions
{
    public static IServiceProvider ApplyDatabaseMigrations(this IServiceProvider app, bool reset = false,
        int? version = null)
    {
        const int retryInterval = 1; // seconds
        const int retryAttempts = 10;
        var logs = app.GetRequiredService<ILogger<IMigrationRunner>>();

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryAttempts,
                retryAttempt => TimeSpan.FromSeconds(retryInterval),
                (exception, timeSpan, attempt, context) =>
                    logs.LogWarning(
                        $"⚠️ Attempt {attempt} of {retryAttempts} failed with exception {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));

        policy.Execute(() =>
        {
            using var scope = app.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            logs.LogInformation("💾 Migrating database schema");
            if (reset) runner.MigrateDown(0);

            if (version.HasValue)
                runner.MigrateUp(version.Value);
            else
                runner.MigrateUp();

            logs.LogInformation("️💾 Migrated database schema");
        });

        return app;
    }
}