namespace Web.AcceptanceTests.Pages.Components;

public class AlertComponent(IPage page)
{
    private readonly ILocator _alert = page.GetByTestId("alert");

    public async Task AssertLevelIs(string level) =>
        await Assertions
            .Expect(_alert)
            .ToHaveClassAsync($"alert alert-{level}");

    public async Task AssertMessageContains(string message)
    {
        var content = await _alert.InnerTextAsync();
        content.ToLowerInvariant().Should().Contain(message.ToLowerInvariant());
    }

    public async Task AssertWarningContains(string message)
    {
        await AssertLevelIs("warning");
        await AssertMessageContains(message);
    }

    public async Task AssertSuccessContains(string message)
    {
        await AssertLevelIs("success");
        await AssertMessageContains(message);
    }

    public async Task AssertDangerContains(string message)
    {
        await AssertLevelIs("danger");
        await AssertMessageContains(message);
    }
}