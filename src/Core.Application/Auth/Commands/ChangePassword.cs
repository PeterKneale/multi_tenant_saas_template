using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Commands;

public static class ChangePassword
{
    [SensitiveData]
    public record Command(string OldPassword, string NewPassword) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OldPassword).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
            RuleFor(m => m.NewPassword).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
        }
    }

    public class Handler(
        ICurrentContext context,
        IReadOnlyRepository<User> repository,
        IPasswordCheck checker,
        IPasswordHash hasher,
        ILogger<Handler> logs) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = context.UserId;

            var user = await repository.SingleOrDefaultAsync(new GetUserByIdSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            logs.LogInformation("Changing password for user {Name} ({Email})", user.Name, user.Email);
            user.ChangePassword(command.OldPassword, command.NewPassword, checker, hasher);
        }
    }
}