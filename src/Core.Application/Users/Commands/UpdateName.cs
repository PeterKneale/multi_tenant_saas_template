using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Commands;

public static class UpdateName
{
    public record Command(Guid UserId, string FirstName, string LastName) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(UserConstants.MaxFirstNameLength);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(UserConstants.MaxLastNameLength);
        }
    }

    public class Handler(IReadOnlyRepository<User> repository) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var userId = new UserId(command.UserId);

            var user = await repository.SingleOrDefaultAsync(new GetUserByIdSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            var name = new Name(command.FirstName, command.LastName);
            user.ChangeName(name);
        }
    }
}