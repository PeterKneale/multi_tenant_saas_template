namespace Core.Infrastructure.Emails.Models;

public record VerifyEmailAddressEmail : BaseEmailWithAction
{
    public string ToName { get; init; }
}