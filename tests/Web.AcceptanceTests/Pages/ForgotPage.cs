namespace Web.AcceptanceTests.Pages;

public class ForgotPage(IPage page) : BasePageModel(page)
{
    private readonly ILocator _button = page.GetByTestId("ForgotButton");
    private readonly ILocator _email = page.GetByTestId("Email");

    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Forgot Password");

    public static async Task<ForgotPage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/Auth/Forgot");
        return new ForgotPage(page);
    }

    public async Task EnterEmail(string email) =>
        await _email.FillAsync(email);

    public async Task ClickForgotButton() =>
        await _button.ClickAsync();
}