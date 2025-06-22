using Core.Domain.Features.Contracts;

namespace Core.Domain.Features.Rules;

internal class NameMustNotBeUsed(FeatureName name, IFeatureNameCheck check) : IBusinessRule
{
    public string Message => "Feature name is used";

    public bool IsBroken()
    {
        return check.NameExists(name);
    }
}