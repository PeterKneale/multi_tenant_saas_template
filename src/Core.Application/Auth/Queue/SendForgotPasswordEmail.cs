using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Queue;

public static class SendForgotPasswordEmail
{
    public record Command(UserId UserId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler(IReadOnlyRepository<User> users, IEmailService sender) : IRequestHandler<Command>
    {
        public async Task Handle(Command notification, CancellationToken cancellationToken)
        {
            var userId = notification.UserId;

            var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            await sender.SendForgotPassword(user);
        }
    }
}