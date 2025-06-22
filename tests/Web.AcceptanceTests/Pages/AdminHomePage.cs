namespace Web.AcceptanceTests.Pages;

public class AdminHomePage(IPage page) : BasePageModel(page)
{
    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Admin");

    public static async Task<HomePage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/admin");
        return new HomePage(page);
    }
}