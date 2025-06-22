using Core.Domain.Organisations;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Projects.Contracts;

namespace Core.Domain.Projects.Rules;

internal class NameMustNotBeUsed(ProjectName name, IProjectNameCheck check) : IBusinessRule
{
    public string Message => "Organisation name is used";

    public bool IsBroken()
    {
        return check.NameExists(name);
    }
}