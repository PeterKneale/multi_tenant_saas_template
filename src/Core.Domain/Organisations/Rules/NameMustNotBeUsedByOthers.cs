using Core.Domain.Organisations.Contracts;

namespace Core.Domain.Organisations.Rules;

internal class NameMustNotBeUsedByOthers(OrganisationId id, OrganisationName name, IOrganisationNameCheck check)
    : IBusinessRule
{
    public string Message => $"The organisation name '{name.Title}' is in use by another organisation";

    public bool IsBroken()
    {
        return check.AnyOtherOrganisationUsesName(id, name);
    }
}