using Web.AcceptanceTests.Pages;

namespace Web.AcceptanceTests.Features.Authentication;

[Binding]
[Scope(Tag = "authentication")]
public class Steps(IPage page)
{
    private LoginPage _login;

    [Given("I navigate to the login page")]
    public async Task GivenINavigateToTheLoginPage()
    {
        _login = await LoginPage.GotoAsync(page);
    }
    
    [Given("I complete the login form")]
    public async Task GivenICompleteTheLoginForm(Table table)
    {
        await _login.AssertCorrectPageAsync();

        var data = table.CreateInstance<Form>();
        if (!string.IsNullOrEmpty(data.Email))
            await _login.EnterEmail(data.Email);
        if (!string.IsNullOrEmpty(data.Password))
            await _login.EnterPassword(data.Password);
    }
    
    [Given("I enter '(.*)' in the honeypot field")]
    public async Task GivenIEnterInTheHoneypotField(string value) =>
        await _login.EnterHoneyPot(value);
    
    [When("I click the login button")]
    public async Task WhenIClickTheLoginButton() =>
        await _login.ClickLoginButton();

    [Then("the Email field status should be (.*)")]
    public async Task ThenTheEmailFieldStatusShouldBe(string valid) =>
        await _login.AssertEmailFieldStatus(valid.ToBoolean());

    [Then("the Password field status should be (.*)")]
    public async Task ThenThePasswordFieldStatusShouldBe(string valid) =>
        await _login.AssertPasswordFieldStatus(valid.ToBoolean());
    
    [Then("the Email field should be required")]
    public async Task ThenTheEmailFieldShouldBeRequired()
    {
        var required = await _login.IsEmailFieldRequiredAsync();
        required.Should().BeTrue("because the Email field should be required for login");
    }
    
    [Then("the Password field should be required")]
    public async Task ThenThePasswordFieldShouldBeRequired()
    {
        var required = await _login.IsPasswordFieldRequiredAsync();
        required.Should().BeTrue("because the Password field should be required for login");
    }

    [Then("the Email label should indicate the field is required")]
    public async Task ThenTheEmailLabelShouldIndicateTheFieldIsRequired() => await _login.AssertEmailLabelIndicatesFieldRequired();

    [Then("the Password label should indicate the field is required")]
    public async Task ThenThePasswordLabelShouldIndicateTheFieldIsRequired() => await _login.AssertPasswordLabelIndicatesFieldRequired();
    
    [Then("I should see an '(.*)' alert containing '(.*)'")]
    public async Task ThenIShouldSeeAnAlertContaining(string level, string message)
    {
        await _login.Alert.AssertLevelIs(level);
        await _login.Alert.AssertMessageContains(message);
    }

    [Then("I should be on the home page")]
    public async Task ThenIShouldBeOnTheHomePage()
    {
        var homePage = new HomePage(page);
        await homePage.AssertCorrectPageAsync();
    }

    [Then("I should be on the admin home page")]
    public async Task ThenIShouldBeOnTheAdminHomePage()
    {
        var adminHomePage = new AdminHomePage(page);
        await adminHomePage.AssertCorrectPageAsync();
    }
}