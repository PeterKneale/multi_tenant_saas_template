using Core.Application.Contracts;
using Core.Domain.Features;
using Core.Domain.Features.Contracts;
using Core.Domain.Features.Specifications;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Projects;
using Core.Domain.Projects.Contracts;
using Core.Domain.Projects.Specifications;

namespace Core.Infrastructure.Services;

public class NameCheckService(
    IReadOnlyRepository<Project> projects,
    IReadOnlyRepository<Feature> features,
    ILogger<NameCheckService> log)
    : IProjectNameCheck, IFeatureNameCheck

{
    public bool NameExists(ProjectName name)
    {
        return projects.AnyAsync(new GetProjectByName(name)).GetAwaiter().GetResult();
    }

    public bool NameExists(FeatureName name)
    {
        return features.AnyAsync(new GetFeatureByName(name)).GetAwaiter().GetResult();
    }
}