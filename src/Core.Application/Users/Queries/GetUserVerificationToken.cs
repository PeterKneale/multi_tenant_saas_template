using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Queries;

public static class GetUserVerificationToken
{
    public record Query(Guid UserId) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<User> users) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            var userId = new UserId(query.UserId);

            var user = await users.SingleOrDefaultAsync(new GetUserByIdGlobalSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            if (user.VerifiedToken == null) throw new UserAlreadyVerifiedException(userId);

            return user.VerifiedToken;
        }
    }
}