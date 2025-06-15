using Core.Domain.UnitTests.Users.Fakes;
using Core.Domain.Users;

namespace Core.Domain.UnitTests.Users;

public class RoleTests
{
    [Fact]
    public void Active_users_can_have_their_role_set()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;
        user.VerifyAndActivate(verification);

        // act
        user.SetRole(UserRole.Owner);

        // assert
        user.Role.Should().Be(UserRole.Owner);
    }

    [Fact]
    public void Attempting_to_set_the_role_of_an_unverified_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();

        // act
        var act = () => user.SetRole(UserRole.Owner);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }

    [Fact]
    public void Attempting_to_set_the_role_of_an_inactive_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();
        user.VerifyAndActivate(user.VerifiedToken);
        user.Deactivate(FakeUserContext.WithRandomUserId());

        // act
        var act = () => user.SetRole(UserRole.Owner);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an active user");
    }
}