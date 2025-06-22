namespace Core.Domain.Features.Contracts;

public interface IFeatureNameCheck
{
    bool NameExists(FeatureName name);
}