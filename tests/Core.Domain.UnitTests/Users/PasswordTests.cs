using Core.Domain.UnitTests.Users.Fakes;
using Core.Domain.Users;
using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

public class PasswordTests
{
    private readonly IPasswordCheck _checker;
    private readonly IPasswordHash _hasher;
    private readonly string _password;
    private readonly User _user;

    public PasswordTests()
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
    public void Password_is_set()
    {
        _user.Password.Should().NotBeEmpty();
    }

    [Fact]
    public void Password_can_be_validated_if_user_verified_and_active()
    {
        VerifyUser();
        _user.CanUserLogin(_password, _checker);
    }

    [Fact]
    public void Password_can_not_be_validated_if_user_verified_and_active_but_password_is_wrong()
    {
        VerifyUser();
        var success = _user.CanUserLogin("wrong_password", _checker);
        success.Should().BeFalse();
    }

    [Fact]
    public void Password_cannot_be_verified_if_user_is_unverified()
    {
        var act = () => _user.CanUserLogin(_password, _checker);
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }

    [Fact]
    public void Password_cannot_be_verified_if_user_is_inactive()
    {
        VerifyUser();
        DeactivateUser();
        var act = () => _user.CanUserLogin(_password, _checker);
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an active user");
    }

    private void VerifyUser()
    {
        _user.VerifyAndActivate(_user.VerifiedToken);
    }

    private void DeactivateUser()
    {
        _user.Deactivate(FakeUserContext.WithRandomUserId());
    }
}