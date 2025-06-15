using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Commands;

public static class ActivateUser
{
    public record Command(Guid UserId) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler(IReadOnlyRepository<User> repository) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);

            var user = await repository.SingleOrDefaultAsync(new GetUserByIdSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            user.Activate();
        }
    }
}