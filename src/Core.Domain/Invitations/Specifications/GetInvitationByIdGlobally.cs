namespace Core.Domain.Invitations.Specifications;

public class GetInvitationByIdGlobally : GlobalSingleResultSpecification<Invitation>
{
    public GetInvitationByIdGlobally(InvitationId id)
    {
        Query.Where(x => x.Id.Equals(id));
    }
}