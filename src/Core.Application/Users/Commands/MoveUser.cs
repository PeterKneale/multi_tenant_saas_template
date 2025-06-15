using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Commands;

public static class MoveUser
{
    public record Command(Guid UserId, Guid OrganisationId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler(
        IReadOnlyRepository<Organisation> organisations,
        IRepository<User> users,
        IRepository<Invitation> invitations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);
            var organisationId = new OrganisationId(command.OrganisationId);

            var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            var organisation = await organisations.SingleOrDefaultAsync(new GetOrganisationByIdGlobally(organisationId),
                cancellationToken);
            if (organisation == null) throw new OrganisationNotFoundException(organisationId);

            var usersInvitations =
                await invitations.ListAsync(new ListInvitationsByUserGlobally(userId), cancellationToken);

            user.ChangeOrganisation(organisationId);
            await users.UpdateAsync(user, cancellationToken);

            await invitations.DeleteRangeAsync(usersInvitations, cancellationToken);
        }
    }
}