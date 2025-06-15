namespace Core.Domain.Users.Specifications;

public class GetUserByIdGlobalSpecification : GlobalSingleResultSpecification<User>
{
    public GetUserByIdGlobalSpecification(UserId userId)
    {
        Query.Where(x => x.Id.Equals(userId));
    }
}