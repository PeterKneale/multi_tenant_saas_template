namespace Core.Infrastructure.Emails.Models;

public record ForgotPasswordEmail : BaseEmailWithAction
{
    public string ToName { get; init; }
}