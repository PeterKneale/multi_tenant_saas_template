namespace Core.Domain.Users.Specifications;

public class GetUserByIdSpecification : SingleResultSpecification<User>
{
    public GetUserByIdSpecification(UserId userId)
    {
        Query
            .Where(x => x.Id.Equals(userId));
    }
}