namespace Core.Infrastructure.Emails.Models;

public record UserInvitationEmail : BaseEmailWithAction
{
    public string InviterOrganisationName { get; init; }
    public string InviterUserName { get; init; }
}