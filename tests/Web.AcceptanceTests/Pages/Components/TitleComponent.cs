namespace Web.AcceptanceTests.Pages.Components;

public class TitleComponent(IPage page)
{
    public async Task AssertTitleAsync(string expected)
    {
        var actual = await GetContentAsync("page-title");
        actual = actual.Trim().ToLowerInvariant();
        actual.Should().Be(expected.ToLowerInvariant(), $"Expected page {expected} but found {actual}");
    }

    private async Task<string?> GetContentAsync(string selector) =>
        await page
            .GetByTestId(selector)
            .GetAttributeAsync("data-value");
}