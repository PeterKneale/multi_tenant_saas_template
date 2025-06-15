using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;

namespace Core.Application.Invitations.Queries;

public static class ListInvitations
{
    public record Query : IRequest<IEnumerable<Result>>;

    public class Validator : AbstractValidator<Query>;

    public record Result(Guid Id, string Email);

    private class Handler(IReadOnlyRepository<Invitation> invitations) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken cancellationToken)
        {
            var list = await invitations.ListAsync(new ListOrderedByEmail(), cancellationToken);

            return list.Select(x => new Result(x.Id.Value, x.Email));
        }
    }
}