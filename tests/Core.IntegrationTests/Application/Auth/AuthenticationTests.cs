using Core.Application.Auth.Queries;

namespace Core.IntegrationTests.Application.Auth;

public class AuthenticationTests(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Can_authenticate()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email;
        var password = AdminUserContext.Password;

        // act
        var result = await QueryWithAdminContext(new CanAuthenticate.Command(email, password));

        // assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Can_authenticate_with_incorrect_casing_of_email_address()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email.ToUpperInvariant();
        var password = AdminUserContext.Password;

        // act
        var result = await QueryWithAdminContext(new CanAuthenticate.Command(email, password));

        // assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Cant_authenticate_if_email_is_wrong()
    {
        // arrange
        var email = "wrong_email@example.com";
        var password = AdminUserContext.Password;

        // act
        var result = await Query(new CanAuthenticate.Command(email, password));

        // assert
        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Cant_authenticate_if_password_is_wrong()
    {
        // arrange
        var email = AdminUserContext.Email;
        var password = "wrong_password";

        // act
        var result = await Query(new CanAuthenticate.Command(email, password));

        // assert
        result.Status.Should().Be(ResultStatus.Unauthorized);
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Can_authenticate_after_setting_password()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var email = AdminUserContext.Email;
        var password = Guid.NewGuid().ToString();

        // act
        await CommandWithAdminContext(new SetUserPassword.Command(userId, password));
        var result = await QueryWithAdminContext(new CanAuthenticate.Command(email, password));

        // assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.UserId.Should().Be(userId);
    }
}