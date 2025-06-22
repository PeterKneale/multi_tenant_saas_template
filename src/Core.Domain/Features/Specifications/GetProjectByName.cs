namespace Core.Domain.Features.Specifications;

public class GetFeatureByName : SingleResultSpecification<Feature>
{
    public GetFeatureByName(FeatureName name)
    {
        Query.Where(x => x.Name.Title.Equals(name.Title));
    }
}