namespace Core.Domain.Invitations.Specifications;

public class ListInvitationsByUserGlobally : GlobalSpecification<Invitation>
{
    public ListInvitationsByUserGlobally(UserId id)
    {
        Query.Where(x => x.UserId.Equals(id));
    }
}