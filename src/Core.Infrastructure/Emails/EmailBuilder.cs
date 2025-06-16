using Core.Domain.Invitations;
using Core.Domain.Organisations;
using Core.Domain.Users;
using Core.Infrastructure.Emails.Models;
using ForgotPassword = Core.Application.Auth.Commands.ForgotPassword;
using VerifyEmailAddress = Core.Application.Users.Commands.VerifyEmailAddress;

namespace Core.Infrastructure.Emails;

public class EmailBuilder(EmailBuilderOptions options)
{
    public VerifyEmailAddressEmail BuildVerifyEmailAddress(User user) => BuildTemplateModel<VerifyEmailAddressEmail>(user.Email)
            with
            {
                ToName = user.Name.FullName,
                ActionUrl = BuildSiteRelativeUrl($"/Auth/Verify?UserId={user.Id}&verification={user.VerifiedToken}"),
                ActionText = "Verify Email Address"
            };

    public ForgotPasswordEmail BuildForgotPassword(User user) => BuildTemplateModel<ForgotPasswordEmail>(user.Email)
            with
            {
                ToName = user.Name.FullName,
                ActionUrl = BuildSiteRelativeUrl($"/Auth/Reset?UserId={user.Id}&Token={user.ForgottenToken}"),
                ActionText = "Reset Password"
            };

    public UserInvitationEmail BuildUserInvitation(Organisation organisation, User user, Invitation invitation) => BuildTemplateModel<UserInvitationEmail>(invitation.Email)
            with
            {
                ActionUrl = BuildSiteRelativeUrl($"/Auth/Accept?InvitationId={invitation.Id}"),
                ActionText = "Accept Invitation",
                InviterOrganisationName = organisation.Name.Title,
                InviterUserName = user.Name.FullName
            };
    private T BuildTemplateModel<T>(EmailAddress emailAddress) where T : BaseEmail, new() =>
        new()
        {
            ToEmail = emailAddress,
            FromEmail = options.FromEmail,
            ProductUrl = options.PublicUri.ToString(),
            ProductName = options.ProductName,
            SupportEmail = options.SupportEmail.ToLowerInvariant(),
            HelpUrl = BuildHelpUri(options).ToString()
        };

    private string BuildSiteRelativeUrl(string uri) =>
        new Uri(options.PublicUri, new Uri(uri, UriKind.Relative)).ToString();

    private static Uri BuildHelpUri(EmailBuilderOptions emailBuilder) =>
        new(emailBuilder.PublicUri, new Uri(emailBuilder.HelpUrl, UriKind.Relative));
}