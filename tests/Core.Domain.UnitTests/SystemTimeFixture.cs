namespace Core.Domain.UnitTests;

public class SystemTimeFixture : IDisposable
{
    // Fixtures that require managing the system time 
    // need to use this collection fixture. This prevents tests
    // overwriting each others faked system times
    public SystemTimeFixture()
    {
        SystemTime.ResetDateTime();
    }

    public void Dispose()
    {
        SystemTime.ResetDateTime();
    }

    public void SetDateTime(DateTimeOffset dateTime)
    {
        SystemTime.SetDateTime(dateTime);
    }
}