namespace Core.Domain.Users.Specifications;

public class GetUsersByIdsSpecification : SingleResultSpecification<User>
{
    public GetUsersByIdsSpecification(IEnumerable<UserId> userIds)
    {
        Query.Where(user => userIds.Contains(user.Id));
    }
}