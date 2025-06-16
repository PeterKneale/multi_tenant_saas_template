namespace Core.Infrastructure.Emails;

public class EmailBuilderOptions
{
    public const string SectionName = "EmailBuilder";
    public required string FromEmail { get; init; }
    public string SupportEmail { get; init; } = null!;
    public required string HelpUrl { get; init; }
    public required Uri PublicUri { get; init; }
    public required string ProductName { get; init; }
}