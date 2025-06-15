namespace Core.IntegrationTests.Application.Users;

public class MoveUserTests(ServiceFixture service, ITestOutputHelper output) : MultipleOrganisationTest(service, output)
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task User_can_be_moved_from_org_to_org(bool admin)
    {
        // Move the member of organisation 1 to organisation 2
        var organisation1 = Org1Admin.OrganisationId;
        var organisation2 = Org2Admin.OrganisationId;
        var userContext = admin ? Org1Admin : Org1Member;
        var memberId = userContext.UserId;
        var invitationId = await CreateInvitation(userContext);

        // act
        await MoveUser(memberId, organisation1, organisation2);

        // assert user is moved
        await AssertUserDoesNotBelong(memberId, Org1Admin);
        await AssertUserBelongs(memberId, Org2Admin);
        // assert invitations are deleted
        await AssertInvitationDoesNotBelong(invitationId, Org1Admin);
        await AssertInvitationDoesNotBelong(invitationId, Org2Admin);
    }

    private async Task MoveUser(Guid memberId, Guid organisation1, Guid organisation2)
    {
        Output.WriteLine($"Moving member {memberId} from {organisation1} to {organisation2}");
        await Command(new MoveUser.Command(memberId, organisation2));
    }


    private async Task<Guid> CreateInvitation(UserContext context)
    {
        var invitationId = Guid.NewGuid();
        var email = $"user+{Guid.NewGuid().ToString()[..6]}@example.com";
        await Command(new CreateInvitation.Command(invitationId, email), context);
        return invitationId;
    }

    private async Task AssertUserDoesNotBelong(Guid memberId, UserContext context)
    {
        (await Query(new ListUsers.Query(), context)).ToList()
            .Should().NotContain(x => x.Id == memberId);
    }

    private async Task AssertUserBelongs(Guid memberId, UserContext context)
    {
        (await Query(new ListUsers.Query(), context)).ToList()
            .Should().ContainSingle(x => x.Id == memberId);
    }

    private async Task AssertInvitationBelongs(Guid invitationId, UserContext context)
    {
        (await Query(new ListInvitations.Query(), context)).ToList()
            .Should().ContainSingle(x => x.Id == invitationId);
    }

    private async Task AssertInvitationDoesNotBelong(Guid invitationId, UserContext context)
    {
        (await Query(new ListInvitations.Query(), context)).ToList()
            .Should().NotContain(x => x.Id == invitationId);
    }
}