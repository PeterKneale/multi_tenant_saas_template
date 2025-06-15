namespace Core.Domain.Invitations.Specifications;

public class GetInvitationByEmail : SingleResultSpecification<Invitation>
{
    public GetInvitationByEmail(EmailAddress email)
    {
        Query.Where(x => x.Email.Equals(email));
    }
}