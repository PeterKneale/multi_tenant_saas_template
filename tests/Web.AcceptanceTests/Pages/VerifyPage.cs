namespace Web.AcceptanceTests.Pages;

public class VerifyPage(IPage page) : BasePageModel(page)
{
    public static async Task<VerifyPage> GotoVerifyPage(IPage page, Guid userId, Guid verification)
    {
        await page.GotoRelativeUrlAsync($"/Auth/Verify?UserId={userId}&Verification={verification}");
        return new VerifyPage(page);
    }

    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Verify");
}