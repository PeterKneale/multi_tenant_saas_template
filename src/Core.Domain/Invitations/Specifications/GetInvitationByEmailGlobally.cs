namespace Core.Domain.Invitations.Specifications;

public class GetInvitationByEmailGlobally : GlobalSingleResultSpecification<Invitation>
{
    public GetInvitationByEmailGlobally(EmailAddress email)
    {
        Query.Where(x => x.Email.Equals(email));
    }
}