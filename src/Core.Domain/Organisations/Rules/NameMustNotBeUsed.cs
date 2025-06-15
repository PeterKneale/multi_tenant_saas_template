using Core.Domain.Organisations.Contracts;

namespace Core.Domain.Organisations.Rules;

internal class NameMustNotBeUsed(OrganisationName name, IOrganisationNameCheck check) : IBusinessRule
{
    public string Message => "Organisation name is used";

    public bool IsBroken()
    {
        return check.AnyOrganisationUsesName(name);
    }
}