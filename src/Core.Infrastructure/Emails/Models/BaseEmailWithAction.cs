namespace Core.Infrastructure.Emails.Models;

public abstract record BaseEmailWithAction : BaseEmail
{
    public string ActionUrl { get; init; }
    public string ActionText { get; init; }
}