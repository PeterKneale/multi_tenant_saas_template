using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Organisations.Commands;

public static class DeleteOrganisation
{
    public record Command(Guid OrganisationId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OrganisationId).NotEmpty();
        }
    }

    public class Handler(IRepository<Organisation> organisations, IReadOnlyRepository<User> users)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var organisationId = new OrganisationId(command.OrganisationId);

            var organisation = await organisations.SingleOrDefaultAsync(new GetOrganisationByIdGlobally(organisationId),
                cancellationToken);
            if (organisation == null) throw new OrganisationNotFoundException();

            var anyUsers = await users.AnyAsync(new ListUsersByOrganisationGloballySpecification(organisationId),
                cancellationToken);
            if (anyUsers) throw new BusinessRuleBrokenException("Cannot delete an organisation with users");

            await organisations.DeleteAsync(organisation, cancellationToken);
        }
    }
}