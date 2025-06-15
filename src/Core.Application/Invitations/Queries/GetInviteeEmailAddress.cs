using Core.Application.Contracts;
using Core.Application.Invitations.Exceptions;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Specifications;

namespace Core.Application.Invitations.Queries;

public static class GetInviteeEmailAddress
{
    public record Query(Guid InvitationId) : IRequest<string>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(m => m.InvitationId).NotEmpty();
        }
    }

    private class Handler(IReadOnlyRepository<Invitation> invitations) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            var invitationId = new InvitationId(query.InvitationId);

            var invitation =
                await invitations.SingleOrDefaultAsync(new GetInvitationByIdGlobally(invitationId), cancellationToken);

            if (invitation is null) throw new NotFoundException(nameof(Invitation), invitationId.Value);

            var email = invitation.Email.ToString();

            return email;
        }
    }
}