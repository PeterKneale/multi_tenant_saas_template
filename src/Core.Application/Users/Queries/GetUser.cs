using Core.Application.Auth.Exceptions;
using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Queries;

public static class GetUser
{
    public record Query(Guid UserId) : IRequest<Result>;

    public record Result(
        Guid OrganisationId,
        Guid Id,
        string FirstName,
        string LastName,
        string Name,
        string Role,
        string Email,
        bool Verified,
        bool? Active);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<User> repository) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var userId = new UserId(query.UserId);

            var user = await repository.SingleOrDefaultAsync(new GetUserByIdSpecification(userId), cancellationToken);
            if (user == null) throw new UserNotFoundException(userId);

            var organisationId = user.OrganisationId;
            var id = user.Id;
            var firstName = user.Name.FirstName;
            var lastName = user.Name.LastName;
            var fullName = user.Name.FullName;
            var roleName = user.Role;
            var email = user.Email;
            return new Result(organisationId.Value, id.Value, firstName, lastName, fullName, roleName, email,
                user.Verified, user.Active);
        }
    }
}