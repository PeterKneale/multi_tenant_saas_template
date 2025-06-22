namespace Web.AcceptanceTests.Pages;

public class LogoutPage(IPage page) : BasePageModel(page)
{
    public static async Task<LogoutPage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/Auth/Logout");
        return new LogoutPage(page);
    }

    public override Task AssertCorrectPageAsync() => throw new NotImplementedException();
}