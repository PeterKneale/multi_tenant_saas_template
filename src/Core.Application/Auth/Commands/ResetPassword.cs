using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Commands;

public static class ResetPassword
{
    [SensitiveData]
    public record Command(Guid UserId, string Token, string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.Token).NotEmpty();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
        }
    }

    public class Handler(IReadOnlyRepository<User> users, IPasswordHash hasher, ILogger<Handler> log)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);

            var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            log.LogInformation("Resetting password for user {Name} ({Email})", user.Name, user.Email);
            user.ResetPassword(command.Token, command.Password, hasher);
        }
    }
}