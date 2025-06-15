namespace Core.IntegrationTests.Application.Auth;

public class VerificationTests(ServiceFixture service, ITestOutputHelper output)
    : BaseTest(service, output), IClassFixture<ServiceFixture>
{
    [Fact]
    public async Task Can_verify()
    {
        // arrange
        var command = Fake.ValidRegisterCommand();

        // act
        await Command(command);

        var verification = await Query(new GetUserVerificationToken.Query(command.UserId));
        await Command(new VerifyEmailAddress.Command(command.UserId, verification));

        // assert
        var context = new UserContext(command.OrganisationId, command.UserId, command.Email, command.Password);
        var user = await Query(new GetUser.Query(command.UserId), context);
        user.Id.Should().Be(command.UserId);
        user.Email.Should().Be(command.Email.ToLowerInvariant());
        user.Verified.Should().BeTrue();
        user.Active.Should().BeTrue();
    }

    [Fact]
    public async Task Cant_verify_with_wrong_user_id()
    {
        // arrange
        var command = Fake.ValidRegisterCommand();
        var wrongUserId = Guid.NewGuid();

        // act
        await Command(command);
        var verification = await Query(new GetUserVerificationToken.Query(command.UserId));
        var act = async () =>
            await Command(new VerifyEmailAddress.Command(wrongUserId, verification));

        // assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task Cant_verify_with_wrong_verification()
    {
        // arrange
        var command = Fake.ValidRegisterCommand();
        var wrongVerification = Guid.NewGuid().ToString();

        // act
        await Command(command);
        var act = async () =>
            await Command(new VerifyEmailAddress.Command(command.UserId, wrongVerification));

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("The verification token does not match");
    }

    [Fact]
    public async Task Can_verify_multiple_times()
    {
        // arrange
        var command = Fake.ValidRegisterCommand();

        // act
        await Command(command);
        var verification = await Query(new GetUserVerificationToken.Query(command.UserId));
        await Command(new VerifyEmailAddress.Command(command.UserId, verification));
        await Command(new VerifyEmailAddress.Command(command.UserId, verification));

        // assert
        var context = new UserContext(command.OrganisationId, command.UserId, command.Email, command.Password);
        var user = await Query(new GetUser.Query(command.UserId), context);
        user.Id.Should().Be(command.UserId);
        user.Email.Should().Be(command.Email.ToLowerInvariant());
        user.Verified.Should().BeTrue();
        user.Active.Should().BeTrue();
    }
}