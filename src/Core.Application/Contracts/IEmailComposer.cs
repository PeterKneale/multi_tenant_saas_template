using Core.Domain.Invitations;
using Core.Domain.Organisations;
using Core.Domain.Users;

namespace Core.Application.Contracts;

public interface IEmailComposer
{
    Task SendVerifyEmailAddress(User user);
    Task SendForgotPassword(User user);

    Task SendUserInvitation(Organisation organisation, User user, Invitation invitation);
}