using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Invitations.Contracts;
using Core.Domain.Invitations.Specifications;
using Core.Domain.Users;
using Core.Domain.Users.Specifications;

namespace Core.Infrastructure.Services;

public class InvitationEmailCheck(
    IReadOnlyRepository<User> users,
    IReadOnlyRepository<Invitation> invitations,
    ILogger<InvitationEmailCheck> log) : IInvitationEmailCheck
{
    public bool InvitationExists(EmailAddress email)
    {
        log.LogInformation($"🤔 Checking if invitation exists for email {email}");
        return invitations.AnyAsync(new GetInvitationByEmail(email)).GetAwaiter().GetResult();
    }

    public bool EmailExistsGlobally(EmailAddress email)
    {
        log.LogInformation($"🤔 Checking if email exists globally {email}");
        return users.AnyAsync(new GetUserByEmailGlobalSpecification(email)).GetAwaiter().GetResult();
    }
}