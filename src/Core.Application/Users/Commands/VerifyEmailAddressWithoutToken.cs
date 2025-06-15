using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Commands;

public static class VerifyEmailAddressWithoutToken
{
    public record Command(string Email) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
        }
    }

    public class Handler(IRepository<User> users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var email = EmailAddress.Create(command.Email);

            var user = await users.SingleOrDefaultAsync(new GetUserByEmailGlobalSpecification(email),
                cancellationToken);
            if (user == null) throw new UserNotFoundException(email);

            // verify forcefully by retrieving the current verification token and using that.
            var verification = user.VerifiedToken;
            user.VerifyAndActivate(verification);
        }
    }
}