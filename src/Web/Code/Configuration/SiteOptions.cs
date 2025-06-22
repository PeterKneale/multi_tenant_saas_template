namespace Web.Code.Configuration;

public class SiteOptions
{
    public const string SectionName = "Site";
    public string? AdminEmail { get; init; }
    public string? AdminPassword { get; init; }
}