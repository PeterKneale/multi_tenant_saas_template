using Core.Domain.Users;
using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

public class ForgotPasswordTests
{
    private readonly IPasswordHash _hasher;
    private readonly string _password;
    private readonly User _user;

    public ForgotPasswordTests()
    {
        var organisationId = OrganisationId.Create(Guid.NewGuid());
        var userId = UserId.Create(Guid.NewGuid());
        var first = "x";
        var last = "x";
        var name = new Name(first, last);
        var email = EmailAddress.Create("user@example.com");
        _password = "password";
        _hasher = new Mock<IPasswordHash>().Object;

        _user = User.CreateOwner(
            organisationId,
            userId,
            name,
            email,
            _password,
            _hasher
        );
    }

    [Fact]
    public void Token_is_not_set()
    {
        _user.ForgottenToken.Should().BeNull();
    }

    [Fact]
    public void TokenExpiry_is_not_set()
    {
        _user.ForgottenTokenExpiry.Should().BeNull();
    }

    [Fact]
    public void ForgotPassword_sets_token()
    {
        // arrange
        _user.VerifyAndActivate(_user.VerifiedToken);

        // act
        _user.ForgotPassword();

        // assert
        _user.ForgottenToken.Should().NotBeNull();
    }

    [Fact]
    public void ForgotPassword_sets_token_expiry()
    {
        // arrange
        _user.VerifyAndActivate(_user.VerifiedToken);

        // act
        _user.ForgotPassword();

        // assert
        _user.ForgottenTokenExpiry.Should().BeCloseTo(SystemTime.UtcNow(), TimeSpan.FromHours(24));
    }
}