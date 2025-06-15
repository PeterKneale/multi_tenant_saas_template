using Core.Domain.Invitations.Contracts;

namespace Core.Domain.Invitations.Rules;

public class EmailAddressMustNotExistGlobally(EmailAddress email, IInvitationEmailCheck check) : IBusinessRule
{
    public string Message => $"There is already a user with email {email}";

    public bool IsBroken()
    {
        return check.EmailExistsGlobally(email);
    }
}