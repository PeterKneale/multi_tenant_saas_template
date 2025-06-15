using Ardalis.Result;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;
using Core.Domain.Users.Specifications;

namespace Core.Application.Auth.Queries;

public static class CanAuthenticate
{
    [SensitiveData]
    public record Command(string Email, string Password) : IRequest<Result<Response>>;

    public record Response(Guid UserId, Guid OrganisationId, string Name, string Email, string Role);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty().MaximumLength(UserConstants.MaxEmailLength).EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(UserConstants.MaxPasswordLength);
        }
    }

    public class Handler(IReadOnlyRepository<User> users, IPasswordCheck check, ILogger<Handler> log)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var email = EmailAddress.Create(command.Email);
            var password = command.Password;

            var user = await users.SingleOrDefaultAsync(new GetUserByEmailGlobalSpecification(email),
                cancellationToken);

            if (user == null)
            {
                log.LogWarning("🔎 User email not found '{Email}'", email);
                return Result<Response>.NotFound();
            }

            log.LogInformation("🔑 Authenticating user {Name} ({Email})", user.Name, user.Email);

            var result = user.CanUserLogin(password, check);
            return result
                ? BuildResult(user)
                : Result<Response>.Unauthorized();
        }

        private static Result<Response> BuildResult(User user)
        {
            var userId = user.Id;
            var organisationId = user.OrganisationId;
            var name = user.Name.FullName;
            var email = user.Email;
            var role = user.Role;
            return new Response(userId.Value, organisationId.Value, name, email, role);
        }
    }
}