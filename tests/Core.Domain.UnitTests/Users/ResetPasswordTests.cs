using Core.Domain.Users;
using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

[Collection(nameof(SystemTimeFixtureCollection))]
public class ResetPasswordTests
{
    private readonly IPasswordCheck _checker;
    private readonly IPasswordHash _hasher;
    private readonly string _password;
    private readonly User _user;

    public ResetPasswordTests()
    {
        var organisationId = OrganisationId.Create(Guid.NewGuid());
        var userId = UserId.Create(Guid.NewGuid());
        var first = "x";
        var last = "x";
        var name = new Name(first, last);
        var email = EmailAddress.Create("user@example.com");
        _password = "password";
        _checker = new FakePasswordService();
        _hasher = new FakePasswordService();

        _user = User.CreateOwner(
            organisationId,
            userId,
            name,
            email,
            _password,
            _hasher
        );
        _user.VerifyAndActivate(_user.VerifiedToken);
        _user.ForgotPassword();
    }


    [Fact]
    public void ResetPassword_sets_the_password()
    {
        // arrange
        var password = "apple";

        // act
        _user.ResetPassword(_user.ForgottenToken, password, _hasher);

        // assert
        var result = _user.CanUserLogin(password, _checker);
        result.Should().BeTrue();
    }

    [Fact]
    public void ResetPassword_clears_the_token()
    {
        // arrange
        var password = "apple";

        // act
        _user.ResetPassword(_user.ForgottenToken, password, _hasher);

        // assert
        _user.ForgottenToken.Should().BeNull();
    }

    [Fact]
    public void ResetPassword_clears_the_token_expiry()
    {
        // arrange
        var password = "apple";

        // act
        _user.ResetPassword(_user.ForgottenToken, password, _hasher);

        // assert
        _user.ForgottenTokenExpiry.Should().BeNull();
    }
}