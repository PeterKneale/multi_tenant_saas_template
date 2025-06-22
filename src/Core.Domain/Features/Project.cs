using Core.Domain.Features.Contracts;
using Core.Domain.Features.Rules;

namespace Core.Domain.Features;

public class Feature : BaseEntity
{
    public FeatureId FeatureId { get; private init; }
    public ProjectId ProjectId { get; }
    public OrganisationId OrganisationId { get; private init; }
    public UserId CreatedBy { get; private init; }

    private Feature(FeatureId featureId, ProjectId projectId, OrganisationId organisationId, UserId createdBy,
        FeatureName name)
    {
        FeatureId = featureId;
        ProjectId = projectId;
        OrganisationId = organisationId;
        CreatedBy = createdBy;
        CreatedAt = SystemTime.UtcNow();
        Name = name;
    }
    
    private Feature()
    {
        // For EF Core
    }

    public FeatureName Name { get; private set; }

    public DateTimeOffset CreatedAt { get; private init; }

    public static Feature Create(FeatureId featureId, ProjectId projectId, OrganisationId organisationId,
        UserId createdBy, FeatureName name, IFeatureNameCheck check)
    {
        CheckRule(new NameMustNotBeUsed(name, check));
        return new Feature(featureId, projectId, organisationId, createdBy, name);
    }
}