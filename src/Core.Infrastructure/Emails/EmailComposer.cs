using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Organisations;
using Core.Domain.Users;

namespace Core.Infrastructure.Emails;

public class EmailComposer : IEmailComposer
{
    public Task SendVerifyEmailAddress(User user)
    {
        throw new NotImplementedException();
    }

    public Task SendForgotPassword(User user)
    {
        throw new NotImplementedException();
    }

    public Task SendUserInvitation(Organisation organisation, User user, Invitation invitation)
    {
        throw new NotImplementedException();
    }
}