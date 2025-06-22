using Core.Domain.Projects.Contracts;
using Core.Domain.Projects.Rules;

namespace Core.Domain.Projects;

public class Project : BaseEntity
{
    public ProjectId ProjectId { get; private init; }
    public OrganisationId OrganisationId { get; private init; }
    public UserId CreatedBy { get; private init; }

    private Project(ProjectId projectId, OrganisationId organisationId, UserId createdBy, ProjectName name)
    {
        ProjectId = projectId;
        OrganisationId = organisationId;
        CreatedBy = createdBy;
        CreatedAt = SystemTime.UtcNow();
        Name = name;
    }
    private Project()
    {
        // For EF Core
    }

    public ProjectName Name { get; private set; }

    public DateTimeOffset CreatedAt { get; private init; }

    public static Project Create(ProjectId projectId, OrganisationId organisationId, UserId createdBy, ProjectName name, IProjectNameCheck check)
    {
        CheckRule(new NameMustNotBeUsed(name, check));
        return new Project(projectId, organisationId, createdBy, name);
    }
}