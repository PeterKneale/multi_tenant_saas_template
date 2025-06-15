using Core.Application.Contracts;
using Core.Application.Invitations.Queue;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Contracts;
using Core.Domain.Users;

namespace Core.Application.Invitations.Commands;

public static class CreateInvitation
{
    public record Command(Guid InvitationId, string Email) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
        }
    }

    public class Handler(
        IRepository<Invitation> invitations,
        ICurrentContext context,
        IInvitationEmailCheck check,
        ICommandQueue queue) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var invitationId = new InvitationId(command.InvitationId);

            // The email address of the invitee
            var email = EmailAddress.Create(command.Email);

            // The user id of the inviter
            var userId = context.UserId;

            // Users may only invite to their own organisation
            var organisationId = context.OrganisationId;

            var invitation = Invitation.Create(invitationId, organisationId, userId, email, check);

            await invitations.AddAsync(invitation, cancellationToken);

            await queue.QueueMediumPriorityCommand(new SendInvitationEmail.Command(invitation.Id), cancellationToken);
        }
    }
}