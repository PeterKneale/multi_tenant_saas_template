namespace Web.AcceptanceTests.Pages;

public class HomePage(IPage page) : BasePageModel(page)
{
    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Home");

    public static async Task<HomePage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/");
        return new HomePage(page);
    }
}