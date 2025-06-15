namespace Core.Domain;

/// <summary>
///     Used for getting DateTime.UtcNow(), time is changeable for unit testing
///     https://stackoverflow.com/questions/2425721/unit-testing-datetime-now
/// </summary>
[ExcludeFromCodeCoverage]
public static class SystemTime
{
    /// <summary>
    ///     Normally this is a pass-through to DateTimeOffset.UtcNow, but it can be overridden with SetDateTime( .. ) for
    ///     testing or
    ///     debugging.
    /// </summary>
    public static Func<DateTimeOffset> UtcNow = () => DateTimeOffset.UtcNow;

    /// <summary>
    ///     Set time to return when SystemTime.UtcNow() is called.
    /// </summary>
    public static void SetDateTime(DateTimeOffset dateTime)
    {
        UtcNow = () => dateTime;
    }

    /// <summary>
    ///     Resets SystemTime.UtcNow() to return DateTime.UtcNow.
    /// </summary>
    public static void ResetDateTime()
    {
        UtcNow = () => DateTimeOffset.UtcNow;
    }
}