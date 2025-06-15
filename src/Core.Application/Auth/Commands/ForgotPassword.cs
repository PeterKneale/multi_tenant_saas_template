using Core.Application.Auth.Exceptions;
using Core.Application.Auth.Queue;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Commands;

public static class ForgotPassword
{
    public record Command(string Email) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
        }
    }

    public class Handler(IReadOnlyRepository<User> users, ICommandQueue queue) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var email = EmailAddress.Create(command.Email);

            var user = await users.SingleOrDefaultAsync(new GetUserByEmailGlobalSpecification(email),
                cancellationToken);
            if (user == null) throw new UserNotFoundException(email);

            user.ForgotPassword();

            await queue.QueueHighPriorityCommand(new SendForgotPasswordEmail.Command(user.Id), cancellationToken);
        }
    }
}