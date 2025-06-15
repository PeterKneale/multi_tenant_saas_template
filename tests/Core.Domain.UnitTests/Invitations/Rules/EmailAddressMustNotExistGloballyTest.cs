using Core.Domain.Invitations.Contracts;
using Core.Domain.Invitations.Rules;

namespace Core.Domain.UnitTests.Invitations.Rules;

public class EmailAddressMustNotExistGloballyTest
{
    [Fact]
    public void Check_message_formatting()
    {
        var email = EmailAddress.Create("user@domain.com");
        var check = null as IInvitationEmailCheck;
        var rule = new EmailAddressMustNotExistGlobally(email, check!);
        rule.Message.Should().Be("There is already a user with email user@domain.com");
    }
}