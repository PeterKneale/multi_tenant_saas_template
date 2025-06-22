using Polly;

namespace Web.AcceptanceTests;

public static class Eventually
{
    private const int RetryCount = 5;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(1);

    public static async Task ShouldPass(Func<Task> action, string? message = "unknown") =>
        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                RetryCount,
                _ => RetryDelay,
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .ExecuteAsync(action);

    public static void ShouldPass(Action action, string? message = "unknown") =>
        Policy
            .Handle<Exception>()
            .WaitAndRetry(
                RetryCount,
                _ => RetryDelay,
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .Execute(action);

    public static async Task ShouldPassAsync(Func<Task> action, string? message = "unknown") =>
        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                RetryCount,
                _ => RetryDelay,
                (result, timespan, retryNo, context) => { Log(retryNo, result, timespan, message); }
            )
            .ExecuteAsync(action);

    private static void Log(int retryNo, Exception result, TimeSpan timespan, string? message)
    {
        Console.WriteLine($"Waiting for condition '{message}'. RetryNo: {retryNo}. Message: {result.Message}. Delaying: {timespan.TotalSeconds} seconds");
    }
}