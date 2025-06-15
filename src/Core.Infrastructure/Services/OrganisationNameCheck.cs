using Core.Application.Contracts;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Organisations.Specifications;

namespace Core.Infrastructure.Services;

public class OrganisationNameCheck(IReadOnlyRepository<Organisation> repo, ILogger<OrganisationNameCheck> log)
    : IOrganisationNameCheck
{
    public bool AnyOrganisationUsesName(OrganisationName name)
    {
        return repo.AnyAsync(new GetOrganisationByNameGlobalSpecification(name)).GetAwaiter().GetResult();
    }

    public bool AnyOtherOrganisationUsesName(OrganisationId id, OrganisationName name)
    {
        log.LogInformation($"🤔 Checking if organisation with name {name.Title} exists");

        var organisation = repo.SingleOrDefaultAsync(new GetOrganisationByNameGlobalSpecification(name)).GetAwaiter()
            .GetResult();
        if (organisation == null)
            // no organisation uses this name
            return false;

        // an organisation uses this name
        return organisation.Id != id;
    }
}