namespace Web.AcceptanceTests.Pages;

public abstract class BasePageModel(IPage page)
{
    protected IPage Page { get; } = page;

    protected TitleComponent Title => new(Page);

    public AlertComponent Alert => new(Page);

    public abstract Task AssertCorrectPageAsync();
}