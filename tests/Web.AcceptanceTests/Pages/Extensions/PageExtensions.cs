namespace Web.AcceptanceTests.Pages.Extensions;

public static class PageExtensions
{
    public static async Task GotoRelativeUrlAsync(this IPage page, string url)
    {
        var root = Config.GetWebUri();
        var uri = new Uri(root, url);
        await page.GotoAsync(uri.ToString());
    }

    public static async Task FillHiddenAsync(this IPage page, string name, string value)
    {
        var command = "(fieldName, fieldValue) => {{ document.querySelector(`input[name='NAME']`).value = 'VALUE'; }}"
            .Replace("NAME", name)
            .Replace("VALUE", value);
        await page.EvaluateAsync(command);
    }
}