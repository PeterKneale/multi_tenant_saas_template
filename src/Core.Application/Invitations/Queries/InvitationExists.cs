using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;

namespace Core.Application.Invitations.Queries;

public static class InvitationExists
{
    public record Query(Guid InvitationId) : IRequest<bool>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<Invitation> invitations) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query query, CancellationToken cancellationToken)
        {
            var invitationId = new InvitationId(query.InvitationId);

            return await invitations.AnyAsync(new GetInvitationByIdGlobally(invitationId), cancellationToken);
        }
    }
}