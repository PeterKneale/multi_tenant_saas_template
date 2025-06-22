using Core.Application.Auth.Queries;

namespace Core.IntegrationTests.UseCases.Auth;

public class ResetPasswordTests(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationWithMembersTest(service, output, 1)
{
    [Fact]
    public async Task Can_reset_owner_password_using_forget_password()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email;
        var password = AdminUserContext.Password;

        // act
        await Command(new ForgotPassword.Command(email));
        var token = await QueryWithAdminContext(new GetUserPasswordToken.Query(userId));
        await Command(new ResetPassword.Command(userId, token, password));

        // assert
        var result = await Query(new CanAuthenticate.Command(email, password));
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Can_reset_member_password_using_forget_password()
    {
        // arrange
        var member = MemberUserContexts.Single();
        var userId = member.UserId;
        var email = member.Email;
        var password = member.Password;

        // act
        await Command(new ForgotPassword.Command(email));
        var token = await QueryWithAdminContext(new GetUserPasswordToken.Query(userId));
        await Command(new ResetPassword.Command(userId, token, password));

        // assert
        var result = await Query(new CanAuthenticate.Command(email, password));
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Can_reset_password_multiple_times_and_use_latest_token()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email;
        var password = "password";

        // act
        await Command(new ForgotPassword.Command(email));
        await Command(new ForgotPassword.Command(email));
        await Command(new ForgotPassword.Command(email));
        await Command(new ForgotPassword.Command(email));
        var token = await QueryWithAdminContext(new GetUserPasswordToken.Query(userId));
        await Command(new ResetPassword.Command(userId, token, password));

        // assert
        var result = await Query(new CanAuthenticate.Command(email, password));
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Cant_reset_password_with_invalid_token()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email;
        var password = "password";
        var wrongToken = "wrong_token";

        // act
        await Command(new ForgotPassword.Command(email));
        var act = async () => await Command(new ResetPassword.Command(userId, wrongToken, password));

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("The forgot token does not match");
    }

    [Fact]
    public async Task Cant_reset_password_unless_requested()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var password = "password";
        var token = "no_token_exists";

        // act
        var act = async () => await Command(new ResetPassword.Command(userId, token, password));

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a user that has forgotten their password");
    }

    [Fact]
    public async Task Inactive_users_cannot_have_their_password_reset()
    {
        // arrange
        var member = MemberUserContexts.Single();
        var memberId = member.UserId;
        var memberEmail = member.Email;

        // act
        await Command(new DeactivateUser.Command(memberId), AdminUserContext);
        var act = async () => await Command(new ForgotPassword.Command(memberEmail));

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an active user");
    }

    [Fact]
    public async Task Unverified_users_cannot_have_their_password_reset()
    {
        // arrange
        var register = Fake.ValidRegisterCommand();
        var email = register.Email;

        // act
        await Command(register);
        var act = async () => await Command(new ForgotPassword.Command(email));

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }
}