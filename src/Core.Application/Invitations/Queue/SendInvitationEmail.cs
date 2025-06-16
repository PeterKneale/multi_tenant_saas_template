using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Invitations.Queue;

public static class SendInvitationEmail
{
    public record Command(InvitationId InvitationId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
        }
    }

    public class Handler(
        IReadOnlyRepository<User> users,
        IReadOnlyRepository<Organisation> organisations,
        IReadOnlyRepository<Invitation> invitations,
        IEmailService emails) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var invitationId = command.InvitationId;

            var invitation =
                await invitations.SingleOrDefaultAsync(new GetInvitationByIdGlobally(invitationId), cancellationToken);
            if (invitation == null) throw new InvitationNotFoundException(invitationId);

            var specification = new GetUserByIdGlobalSpecification(invitation.UserId);
            var user = await users.SingleOrDefaultAsync(specification, cancellationToken);
            if (user == null) throw new UserNotFoundException(invitation.UserId);

            var organisationId = invitation.OrganisationId;

            var organisation = await organisations.SingleOrDefaultAsync(new GetOrganisationByIdGlobally(organisationId),
                cancellationToken);
            if (organisation == null) throw new OrganisationNotFoundException();

            await emails.SendUserInvitation(organisation, user, invitation);
        }
    }
}