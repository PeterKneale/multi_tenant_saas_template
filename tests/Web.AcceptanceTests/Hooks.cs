using Reqnroll.BoDi;
using Web.AcceptanceTests.Pages;

namespace Web.AcceptanceTests;

[Binding]
public class Hooks
{
    [BeforeFeature]
    public static async Task BeforeFeature(IObjectContainer container)
    {
        await WaitForSiteToBeReady();
        
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Config.GetHeadless()
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            AcceptDownloads = true
        });

        var page = await context.NewPageAsync();
        await LoginPage.GotoAsync(page);

        container.RegisterInstanceAs(playwright);
        container.RegisterInstanceAs(browser);
        container.RegisterInstanceAs(page);

        await TestContext.Out.WriteAsync($"Starting feature: {TestContext.CurrentContext.Test.FullName}");
    }

    [AfterFeature]
    public static async Task AfterFeature(IObjectContainer container)
    {
        var browser = container.Resolve<IBrowser>();
        await browser.CloseAsync();
        var playwright = container.Resolve<IPlaywright>();
        playwright.Dispose();
        await TestContext.Out.WriteAsync($"Finished feature: {TestContext.CurrentContext.Test.FullName}");
    }
    
    public static async Task WaitForSiteToBeReady()
    {
        await Eventually.ShouldPass(async () =>
        {
            using var http = new HttpClient();
            // Ensure the web app is ready to receive requests
            (await http.GetAsync(Config.GetWebReadyUri())).EnsureSuccessStatusCode();
        }, "waiting for site to be ready");
    }
}