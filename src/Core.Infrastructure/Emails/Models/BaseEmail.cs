namespace Core.Infrastructure.Emails.Models;

public abstract record BaseEmail
{
    public string ToEmail { get; init; }
    public string FromEmail { get; init; }
    public string ProductUrl { get; init; }
    public string ProductName { get; init; }
    public string SupportEmail { get; init; }
    public string HelpUrl { get; init; }
}