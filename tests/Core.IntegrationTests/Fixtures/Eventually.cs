namespace Core.IntegrationTests.Fixtures;

public static class Eventually
{
    private static readonly int RetryCount = 120;
    private static readonly TimeSpan RetryInterval = TimeSpan.FromMilliseconds(500);

    public static async Task ExceptionShouldNotBeThrown(Func<Task> action, ITestOutputHelper output)
    {
        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(RetryCount, _ => RetryInterval,
                (exception, timespan, retryNo, _) => { Log(output, exception, retryNo, timespan); })
            .ExecuteAsync(action);
    }

    public static async Task ExceptionShouldNotBeThrown<T>(Func<Task> action, ITestOutputHelper output)
        where T : BusinessRuleBrokenException
    {
        await Policy
            .Handle<T>()
            .WaitAndRetryAsync(RetryCount, _ => RetryInterval,
                (exception, timespan, retryNo, _) => { Log(output, exception, retryNo, timespan); })
            .ExecuteAsync(action);
    }

    private static void Log(ITestOutputHelper output, Exception ex, int retryNo, TimeSpan timespan)
    {
        output.WriteLine($"Error: {ex.Message} {retryNo}/{RetryCount} (waiting {timespan.TotalSeconds}s)");
    }
}