using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Contracts;
using Core.Domain.Organisations.Specifications;

namespace Core.Application.Organisations.Commands;

public static class UpdateOrganisationName
{
    public record Command(string Title, string? Description = null) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Title).NotEmpty().MaximumLength(OrganisationConstants.MaxNameLength);
            RuleFor(m => m.Description).MaximumLength(OrganisationConstants.MaxDescriptionLength);
        }
    }

    public class Handler(IRepository<Organisation> organisations, IOrganisationNameCheck check)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var organisation =
                await organisations.SingleOrDefaultAsync(new GetCurrentOrganisation(), cancellationToken);
            if (organisation == null) throw new OrganisationNotFoundException();
            var name = new OrganisationName(command.Title, command.Description);
            organisation.ChangeName(name, check);
        }
    }
}