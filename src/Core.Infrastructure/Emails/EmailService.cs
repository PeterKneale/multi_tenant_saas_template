using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Organisations;
using Core.Domain.Users;
using Core.Infrastructure.Emails.Models;
using PostmarkDotNet;

namespace Core.Infrastructure.Emails;

public class EmailService(EmailBuilder builder, EmailSender sender) : IEmailService
{
    public async Task SendVerifyEmailAddress(User user)
    {
        var model = builder.BuildVerifyEmailAddress(user);
        var message = BuildMessage("verify-email-address-template", model);
        await sender.SendMessage(message);
    }

    public async Task SendForgotPassword(User user)
    {
        var model = builder.BuildForgotPassword(user);
        var message = BuildMessage("forgot-password-template", model);
        await sender.SendMessage(message);
    }

    public async Task SendUserInvitation(Organisation organisation, User user, Invitation invitation)
    {
        var model = builder.BuildUserInvitation(organisation, user, invitation);
        var message = BuildMessage("user-invitation-template", model);
        await sender.SendMessage(message);
    }

    private static TemplatedPostmarkMessage BuildMessage(string template, BaseEmail email,
        PostmarkMessageAttachment? attachment = null)
    {
        var message = new TemplatedPostmarkMessage
        {
            To = email.ToEmail,
            From = email.FromEmail,
            TrackOpens = true,
            MessageStream = "outbound",
            TemplateAlias = template,
            TemplateModel = email
        };
        if (attachment != null)
        {
            message.Attachments = new List<PostmarkMessageAttachment> { attachment };
        }

        return message;
    }

    private static async Task<string> ToBase64(Stream report)
    {
        using var stream = new MemoryStream();
        await report.CopyToAsync(stream);
        return Convert.ToBase64String(stream.ToArray());
    }
}