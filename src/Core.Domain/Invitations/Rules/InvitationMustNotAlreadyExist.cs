using Core.Domain.Invitations.Contracts;

namespace Core.Domain.Invitations.Rules;

public class InvitationMustNotAlreadyExist(EmailAddress email, IInvitationEmailCheck check) : IBusinessRule
{
    public string Message => $"There is already a pending invitation for {email}";

    public bool IsBroken()
    {
        return check.InvitationExists(email);
    }
}