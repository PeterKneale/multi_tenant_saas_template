using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;

namespace Core.Application.Invitations.Commands;

public static class CancelInvitation
{
    public record Command(Guid InvitationId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
        }
    }

    public class Handler(IRepository<Invitation> invitations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var invitationId = new InvitationId(command.InvitationId);

            var invitation =
                await invitations.SingleOrDefaultAsync(new GetInvitationById(invitationId), cancellationToken);
            if (invitation == null) throw new InvitationNotFoundException(invitationId);

            await invitations.DeleteAsync(invitation, cancellationToken);
        }
    }
}