using Core.Domain.Common;
using Core.Domain.Users;
using Core.Infrastructure.Emails;
using Core.Infrastructure.Services;

namespace Core.Infrastructure.UnitTests.Emails;

public static class Builders
{
    public static User BuildUser(UserId? userId = null, string? email = null)
    {
        var organisationId = OrganisationId.Create();
        var name = new Name("First", "Last");
        var emailAddress = EmailAddress.Create(email ?? "user@example.com");
        var password = "password";
        var hasher = new PasswordService();
        var user = User.CreateOwner(organisationId, userId ?? UserId.Create(), name, emailAddress, password, hasher);
        return user;
    }

    public static EmailBuilderOptions BuildEmailOptions(string? supportEmail = null)
    {
        return new EmailBuilderOptions
        {
            ProductName = "Product Name",
            PublicUri = new Uri("http://localhost.com"),
            HelpUrl = "/help",
            FromEmail = "NoReply@localhost.com",
            SupportEmail = supportEmail ?? "help@localhost.com"
        };
    }
}