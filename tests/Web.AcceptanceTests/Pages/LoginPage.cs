namespace Web.AcceptanceTests.Pages;

public class LoginPage(IPage page) : BasePageModel(page)
{
    private readonly ILocator _emailLabel = page.GetByTestId("Email-Label");
    private readonly ILocator _passwordLabel = page.GetByTestId("Password-Label");
    
    private readonly ILocator _emailField = page.GetByTestId("Email");
    private readonly ILocator _passwordField = page.GetByTestId("Password");
    private readonly ILocator _loginButton = page.GetByTestId("LoginButton");

    public static async Task<LoginPage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/Auth/Login");
        return new LoginPage(page);
    }
    
    public async Task AssertEmailLabelIndicatesFieldRequired() =>
        await Assertions.Expect(_emailLabel).LabelMarkedRequiredAsync();
    
    public async Task AssertPasswordLabelIndicatesFieldRequired() =>
        await Assertions.Expect(_passwordLabel).LabelMarkedRequiredAsync();
    
    public async Task<bool> IsEmailFieldRequiredAsync() =>
        await _emailField.IsFieldRequiredAsync();
    
    public async Task<bool> IsPasswordFieldRequiredAsync() =>
        await _passwordField.IsFieldRequiredAsync();

    public async Task EnterEmail(string email) =>
        await _emailField.FillAsync(email);

    public async Task EnterPassword(string password) =>
        await _passwordField.FillAsync(password);

    public async Task EnterHoneyPot(string value) =>
        await Page.FillHiddenAsync("Name", value);

    public async Task ClickLoginButton() =>
        await _loginButton.ClickAsync();

    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Login");

    public async Task AssertEmailFieldStatus(bool valid) =>
        await Assertions.Expect(_emailField).ToBe(valid);

    public async Task AssertPasswordFieldStatus(bool valid) =>
        await Assertions.Expect(_passwordField).ToBe(valid);
}