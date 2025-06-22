namespace Web.AcceptanceTests.Pages;

public class ResetPage(IPage page) : BasePageModel(page)
{
    private readonly ILocator _button = page.GetByTestId("ResetButton");
    private readonly ILocator _password = page.GetByTestId("Password");

    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Reset My Password");

    public static async Task<ResetPage> GotoAsync(IPage page, Guid userId, Guid token)
    {
        await page.GotoRelativeUrlAsync($"/Auth/Reset?UserId={userId}&Token={token}");
        return new ResetPage(page);
    }

    public async Task EnterPassword(string password) =>
        await _password.FillAsync(password);

    public async Task ClickResetButton() =>
        await _button.ClickAsync();
}