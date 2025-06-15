using Core.Domain.UnitTests.Users.Fakes;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

public class ChangePasswordTests
{
    private readonly IPasswordCheck _checker;
    private readonly IPasswordHash _hasher;
    private readonly string _password;
    private readonly User _user;

    public ChangePasswordTests()
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
    }

    [Fact]
    public void Password_can_be_changed()
    {
        // arrange
        var newPassword = "apple";

        // act
        _user.VerifyAndActivate(_user.VerifiedToken);
        _user.ChangePassword(_password, newPassword, _checker, _hasher);

        // assert
        var result = _user.CanUserLogin(newPassword, _checker);
        result.Should().BeTrue();
    }

    [Fact]
    public void Old_password_must_be_correct()
    {
        // arrange
        var newPassword = "apple";

        // act
        _user.VerifyAndActivate(_user.VerifiedToken);
        var act = () => _user.ChangePassword("wrong_password", newPassword, _checker, _hasher);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("The password does not match");
    }

    [Fact]
    public void User_must_be_verified()
    {
        // arrange
        var newPassword = "apple";

        // act
        var act = () => _user.ResetPassword(_password, newPassword, _hasher);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }

    [Fact]
    public void User_must_be_active()
    {
        // arrange
        var password = "apple";

        // act
        _user.VerifyAndActivate(_user.VerifiedToken);
        _user.Deactivate(FakeUserContext.WithRandomUserId());
        var act = () => _user.ResetPassword(_password, password, _hasher);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an active user");
    }
}