namespace Core.Domain.Users.Specifications;

public class ListUsersSpecification : Specification<User>
{
    public ListUsersSpecification()
    {
        Query
            .OrderBy(x => x.Name.FirstName)
            .ThenBy(x => x.Name.LastName);
    }
}