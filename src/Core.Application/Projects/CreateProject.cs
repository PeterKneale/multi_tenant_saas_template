using Core.Application.Contracts;
using Core.Domain.Projects;
using Core.Domain.Projects.Contracts;

namespace Core.Application.Projects;

public static class CreateProject
{
    public record Command(Guid ProjectId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.ProjectId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }

    public class Handler(IRepository<Project> projects, ICurrentContext context, IProjectNameCheck check)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var projectId = new ProjectId(command.ProjectId);
            var userId = context.UserId;
            var organisationId = context.OrganisationId;
            var name = new ProjectName(command.Name);

            var project = Project.Create(projectId, organisationId, userId, name, check);

            await projects.AddAsync(project, cancellationToken);
        }
    }
}