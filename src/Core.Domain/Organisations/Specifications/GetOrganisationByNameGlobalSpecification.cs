namespace Core.Domain.Organisations.Specifications;

public class GetOrganisationByNameGlobalSpecification : GlobalSingleResultSpecification<Organisation>
{
    public GetOrganisationByNameGlobalSpecification(OrganisationName name)
    {
        Query.Where(x => x.Name.Title.Equals(name.Title));
    }
}