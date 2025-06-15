using Core.Domain.Invitations.Contracts;
using Core.Domain.Invitations.Rules;

namespace Core.Domain.UnitTests.Invitations.Rules;

public class InvitationMustNotAlreadyExistTest
{
    [Fact]
    public void Check_message_formatting()
    {
        var email = EmailAddress.Create("user@domain.com");
        var check = null as IInvitationEmailCheck;
        var rule = new InvitationMustNotAlreadyExist(email, check!);
        rule.Message.Should().Be("There is already a pending invitation for user@domain.com");
    }
}