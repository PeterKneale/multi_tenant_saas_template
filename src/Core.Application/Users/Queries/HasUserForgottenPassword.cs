using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Queries;

public static class HasUserForgottenPassword
{
    public record Query(Guid UserId) : IRequest<bool>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<User> repository) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query query, CancellationToken cancellationToken)
        {
            var userId = new UserId(query.UserId);

            var user = await repository.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId),
                cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            return user.ForgottenToken != null;
        }
    }
}