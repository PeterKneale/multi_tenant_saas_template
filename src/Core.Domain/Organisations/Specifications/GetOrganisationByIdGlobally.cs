namespace Core.Domain.Organisations.Specifications;

public class GetOrganisationByIdGlobally : GlobalSingleResultSpecification<Organisation>
{
    public GetOrganisationByIdGlobally(OrganisationId id)
    {
        Query.Where(x => x.Id.Equals(id));
    }
}