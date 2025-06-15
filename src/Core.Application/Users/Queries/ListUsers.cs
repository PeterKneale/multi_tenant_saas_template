using Core.Application.Contracts;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Application.Users.Queries;

public static class ListUsers
{
    public record Query : IRequest<IEnumerable<Result>>;

    public class Validator : AbstractValidator<Query>;

    public record Result(Guid Id, string Name, string Role, string Email, bool Verified, bool? Active);

    private class Handler(IReadOnlyRepository<User> repository) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query message, CancellationToken cancellationToken)
        {
            var list = await repository.ListAsync(new ListUsersSpecification(), cancellationToken);
            return list.Select(Map);
        }

        private static Result Map(User user)
        {
            return new Result(user.Id.Value, user.Name.FullName, user.Role, user.Email, user.Verified, user.Active);
        }
    }
}