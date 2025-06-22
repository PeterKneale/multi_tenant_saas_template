namespace Core.IntegrationTests.UseCases.Auth;

public class ActivationTests(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationWithMembersTest(service, output, 1)
{
    [Fact]
    public async Task Owner_can_deactivate_a_member()
    {
        // arrange
        var memberId = MemberUserContexts.Single().UserId;

        // act
        await Command(new DeactivateUser.Command(memberId), AdminUserContext);

        // assert
        var user = await Query(new GetUser.Query(memberId), AdminUserContext);
        user.Active.Should().BeFalse();
    }

    [Fact]
    public async Task Owner_can_reactivate_a_member()
    {
        // arrange
        var memberId = MemberUserContexts.Single().UserId;

        // act
        await Command(new DeactivateUser.Command(memberId), AdminUserContext);
        await Command(new ActivateUser.Command(memberId), AdminUserContext);

        // assert
        var user = await Query(new GetUser.Query(memberId), AdminUserContext);
        user.Active.Should().BeTrue();
    }
}