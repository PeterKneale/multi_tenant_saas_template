using Core.Domain.UnitTests.Users.Fakes;

namespace Core.Domain.UnitTests.Users;

public class ActivationTests
{
    [Fact]
    public void Initially_users_have_no_activation_status()
    {
        // arrange
        var user = BogusEntities.NewMember();
        user.Active.Should().BeNull("should not be verified");
    }

    [Fact]
    public void Inactive_users_can_be_activated()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;
        user.VerifyAndActivate(verification);
        user.Deactivate(FakeUserContext.WithRandomUserId());

        // act
        user.Activate();

        // assert
        user.Active.Should().BeTrue("the user has been activated");
    }


    [Fact]
    public void Attempting_to_activate_an_unverified_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();

        // act
        var act = () => user.Activate();

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }

    [Fact]
    public void Attempting_to_activate_an_already_active_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;

        // act
        user.VerifyAndActivate(verification);
        var act = () => user.Activate();

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an inactive user");
    }

    [Fact]
    public void Attempting_to_deactivate_an_unverified_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();

        // act
        var act = () => user.Activate();

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on a verified user");
    }

    [Fact]
    public void Attempting_to_deactivate_an_inactive_user_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();
        user.VerifyAndActivate(user.VerifiedToken);
        user.Deactivate(FakeUserContext.WithRandomUserId());

        // act
        var act = () => user.Deactivate(FakeUserContext.WithRandomUserId());

        // assert
        act.Should().Throw<BusinessRuleBrokenException>()
            .WithMessage("This action must be performed on an active user");
    }

    [Fact]
    public void Attempting_to_deactivate_self_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();
        user.VerifyAndActivate(user.VerifiedToken);

        // act
        var act = () => user.Deactivate(FakeUserContext.WithUserId(user.Id));

        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("This action can only be performed on others");
    }
}