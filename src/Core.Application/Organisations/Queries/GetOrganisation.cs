using Core.Application.Contracts;
using Core.Application.Organisations.Exceptions;
using Core.Domain.Organisations;
using Core.Domain.Organisations.Specifications;

namespace Core.Application.Organisations.Queries;

public static class GetOrganisation
{
    public record Query : IRequest<Result>;

    public class Validator : AbstractValidator<Query>;

    public record Result(Guid Id, string Title, string? Description);

    private class Handler(IReadOnlyRepository<Organisation> repository) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var organisation = await repository.SingleOrDefaultAsync(new GetCurrentOrganisation(), cancellationToken);
            if (organisation == null) throw new OrganisationNotFoundException();

            return new Result(organisation.Id.Value, organisation.Name.Title, organisation.Name.Description);
        }
    }
}