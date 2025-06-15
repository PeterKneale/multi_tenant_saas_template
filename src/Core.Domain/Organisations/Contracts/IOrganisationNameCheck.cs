namespace Core.Domain.Organisations.Contracts;

public interface IOrganisationNameCheck
{
    bool AnyOrganisationUsesName(OrganisationName name);
    bool AnyOtherOrganisationUsesName(OrganisationId id, OrganisationName name);
}