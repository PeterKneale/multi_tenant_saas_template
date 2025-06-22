namespace Web.AcceptanceTests.Pages;

public class RegisterPage(IPage page) : BasePageModel(page)
{
    private readonly ILocator _emailField = page.GetByTestId("Email");
    private readonly ILocator _firstNameField = page.GetByTestId("FirstName");
    private readonly ILocator _lastNameField = page.GetByTestId("LastName");
    private readonly ILocator _organisationField = page.GetByTestId("Organisation");
    private readonly ILocator _passwordField = page.GetByTestId("Password");
    private readonly ILocator _registerButton = page.GetByTestId("RegisterButton");

    public static async Task<RegisterPage> GotoAsync(IPage page)
    {
        await page.GotoRelativeUrlAsync("/Auth/Register");
        return new RegisterPage(page);
    }

    public async Task EnterTitle(string name) =>
        await _organisationField.FillAsync(name);

    public async Task EnterFirstName(string firstName) =>
        await _firstNameField.FillAsync(firstName);

    public async Task EnterLastName(string lastName) =>
        await _lastNameField.FillAsync(lastName);

    public async Task EnterEmail(string email) =>
        await _emailField.FillAsync(email);

    public async Task EnterPassword(string password) =>
        await _passwordField.FillAsync(password);

    public async Task EnterHoneyPot(string value) =>
        await Page.FillHiddenAsync("Name", value);

    public async Task AssertTitleFieldStatus(bool valid) =>
        await Assertions.Expect(_organisationField).ToBe(valid);

    public async Task AssertFirstNameFieldStatus(bool valid) =>
        await Assertions.Expect(_firstNameField).ToBe(valid);

    public async Task AssertLastNameFieldStatus(bool valid) =>
        await Assertions.Expect(_lastNameField).ToBe(valid);

    public async Task AssertEmailFieldStatus(bool valid) =>
        await Assertions.Expect(_emailField).ToBe(valid);

    public async Task AssertPasswordFieldStatus(bool valid) =>
        await Assertions.Expect(_passwordField).ToBe(valid);

    public async Task ClickRegisterButton() =>
        await _registerButton.ClickAsync();

    public async Task<RegisteredPage> ClickRegisterButtonAndNavigate()
    {
        await _registerButton.ClickAsync();
        return new RegisteredPage(Page);
    }

    public override async Task AssertCorrectPageAsync() =>
        await Title.AssertTitleAsync("Register");
}