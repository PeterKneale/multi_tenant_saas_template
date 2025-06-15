namespace Core.Domain.UnitTests.Users;

public class VerificationTests
{
    [Fact]
    public void Initially_users_are_not_verified()
    {
        // arrange
        var user = BogusEntities.NewMember();
        user.Verified.Should().BeFalse("should not be verified");
    }

    [Fact]
    public void Unverified_users_can_be_verified()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;

        // act
        user.VerifyAndActivate(verification);

        // assert
        user.Verified.Should().BeTrue("the user has been verified");
    }

    [Fact]
    public void Verifying_clears_the_verification_token()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;

        // act
        user.VerifyAndActivate(verification);

        // assert
        user.VerifiedToken.Should().BeNull("the user has been verified");
    }

    [Fact]
    public void Verifying_a_user_activates_the_user()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;

        // act
        user.VerifyAndActivate(verification);

        // assert
        user.Active.Should().BeTrue("the user has been verified");
    }

    [Fact]
    public void Verifying_a_user_can_only_be_done_once()
    {
        // arrange
        var user = BogusEntities.NewMember();
        var verification = user.VerifiedToken;

        // act
        user.VerifyAndActivate(verification);
        var act = () => user.VerifyAndActivate(verification);

        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("This user has already been verified");
    }

    [Fact]
    public void Attempting_to_verify_with_an_invalid_token_fails()
    {
        // arrange
        var user = BogusEntities.NewMember();

        // act
        var act = () => user.VerifyAndActivate("x");

        // assert
        act.Should().Throw<BusinessRuleBrokenException>().WithMessage("The verification token does not match");
        user.Verified.Should().BeFalse();
    }
}