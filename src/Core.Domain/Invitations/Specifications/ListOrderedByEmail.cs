namespace Core.Domain.Invitations.Specifications;

public class ListOrderedByEmail : Specification<Invitation>
{
    public ListOrderedByEmail()
    {
        Query.OrderBy(x => x.Email);
    }
}