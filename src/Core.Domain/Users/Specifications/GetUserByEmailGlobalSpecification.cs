namespace Core.Domain.Users.Specifications;

public class GetUserByEmailGlobalSpecification : GlobalSingleResultSpecification<User>
{
    public GetUserByEmailGlobalSpecification(EmailAddress email)
    {
        Query
            .Where(x => x.Email.Equals(email));
    }
}