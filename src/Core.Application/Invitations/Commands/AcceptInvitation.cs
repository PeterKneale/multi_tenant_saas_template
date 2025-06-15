using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;
using Core.Domain.Users.Specifications;

namespace Core.Application.Invitations.Commands;

public static class AcceptInvitation
{
    public record Command(
        Guid InvitationId,
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(UserConstants.MaxFirstNameLength);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(UserConstants.MaxLastNameLength);
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
        }
    }

    public class Handler(IRepository<Invitation> invitations, IRepository<User> users, IPasswordHash hash)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var invitationId = new InvitationId(command.InvitationId);

            var invitation =
                await invitations.SingleOrDefaultAsync(new GetInvitationByIdGlobally(invitationId), cancellationToken);
            if (invitation == null) throw new InvitationNotFoundException(invitationId);

            var organisationId = invitation.OrganisationId;
            var userid = new UserId(command.UserId);
            var userName = new Name(command.FirstName, command.LastName);
            var userEmail = EmailAddress.Create(command.Email);

            var userPassword = command.Password;

            if (await users.AnyAsync(new GetUserByIdGlobalSpecification(userid), cancellationToken))
                throw new UserAlreadyExistsException(userid);

            if (await users.AnyAsync(new GetUserByEmailGlobalSpecification(userEmail), cancellationToken))
                throw new UserAlreadyExistsException(userEmail);

            var user = User.CreateMember(organisationId, userid, userName, userEmail, userPassword, hash);

            // Users may choose the email address that they signup with
            // We dont want to force them to use the email address that they were invited with
            user.VerifyAndActivate(user.VerifiedToken);
            await users.AddAsync(user, cancellationToken);

            await invitations.DeleteAsync(invitation, cancellationToken);
        }
    }
}