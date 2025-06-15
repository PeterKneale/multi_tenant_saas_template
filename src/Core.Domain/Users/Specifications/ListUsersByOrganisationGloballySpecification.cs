namespace Core.Domain.Users.Specifications;

public class ListUsersByOrganisationGloballySpecification : GlobalSpecification<User>
{
    public ListUsersByOrganisationGloballySpecification(OrganisationId organisationId)
    {
        Query.Where(x => x.OrganisationId == organisationId);
    }
}