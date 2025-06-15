namespace Core.Domain.Invitations.Contracts;

public interface IInvitationEmailCheck
{
    bool InvitationExists(EmailAddress email);
    bool EmailExistsGlobally(EmailAddress email);
}