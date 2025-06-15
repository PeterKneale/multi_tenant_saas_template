using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Queries;

public static class CanImpersonate
{
    public record Query(string Email) : IRequest<Result>;

    public record Result(Guid UserId, Guid OrganisationId, string Name, string Email, string Role);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<User> users, ILogger<Handler> log) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var emailAddress = EmailAddress.Create(query.Email);
            var user = await users.SingleOrDefaultAsync(new GetUserByEmailGlobalSpecification(emailAddress),
                cancellationToken);
            if (user == null) throw new UserNotFoundException(emailAddress);

            if (user.Verified == false) throw new UserNotVerifiedException(user.Id);

            if (user.Active is false) throw new UserNotActiveException(user.Id);

            log.LogInformation("🔑 Impersonating user {Name} ({Email})", user.Name, user.Email);
            var userId = user.Id;
            var organisationId = user.OrganisationId;
            var name = user.Name.FullName;
            var email = user.Email;
            var role = user.Role;
            return new Result(userId.Value, organisationId.Value, name, email, role);
        }
    }
}