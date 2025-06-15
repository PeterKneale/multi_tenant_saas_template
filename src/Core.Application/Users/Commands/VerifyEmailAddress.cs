using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Commands;

public static class VerifyEmailAddress
{
    public record Command(Guid UserId, string Verification) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Verification).NotEmpty();
        }
    }

    public class Handler(IRepository<User> users) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);
            var verification = command.Verification;

            var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            if (user is { Verified: true, Active: true }) return;

            user.VerifyAndActivate(verification);
        }
    }
}