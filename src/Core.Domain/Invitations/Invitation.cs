using Core.Domain.Invitations.Contracts;
using Core.Domain.Invitations.Rules;

namespace Core.Domain.Invitations;

public class Invitation : BaseEntity
{
    private Invitation()
    {
    }

    public Invitation(InvitationId invitationId, OrganisationId organisationId, UserId userId, EmailAddress email)
    {
        Id = invitationId;
        OrganisationId = organisationId;
        UserId = userId;
        Email = email;
        CreatedAt = SystemTime.UtcNow();
    }

    public InvitationId Id { get; private init; }
    public OrganisationId OrganisationId { get; private init; }
    public UserId UserId { get; private init; }
    public EmailAddress Email { get; private init; }
    public DateTimeOffset CreatedAt { get; private init; }

    public static Invitation Create(InvitationId invitationId, OrganisationId organisationId, UserId userId,
        EmailAddress email, IInvitationEmailCheck check)
    {
        CheckRule(new EmailAddressMustNotExistGlobally(email, check));
        CheckRule(new InvitationMustNotAlreadyExist(email, check));
        return new Invitation(invitationId, organisationId, userId, email);
    }
}