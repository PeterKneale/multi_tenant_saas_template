using Core.Application.Contracts;
using Core.Domain.Features;
using Core.Domain.Features.Contracts;

namespace Core.Application.Features;

public static class CreateFeature
{
    public record Command(Guid ProjectId, Guid FeatureId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.ProjectId).NotEmpty();
            RuleFor(m => m.FeatureId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }

    public class Handler(IRepository<Feature> Features, ICurrentContext context, IFeatureNameCheck check)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var featureId = new FeatureId(command.FeatureId);
            var projectId = new ProjectId(command.ProjectId);
            var userId = context.UserId;
            var organisationId = context.OrganisationId;
            var name = new FeatureName(command.Name);

            var feature = Feature.Create(featureId, projectId, organisationId, userId, name, check);

            await Features.AddAsync(feature, cancellationToken);
        }
    }
}