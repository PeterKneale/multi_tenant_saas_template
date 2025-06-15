namespace Core.Domain.Invitations.Specifications;

public class GetInvitationById : SingleResultSpecification<Invitation>
{
    public GetInvitationById(InvitationId id)
    {
        Query.Where(x => x.Id.Equals(id));
    }
}