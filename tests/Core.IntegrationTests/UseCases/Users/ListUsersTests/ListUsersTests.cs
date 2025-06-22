namespace Core.IntegrationTests.UseCases.Users.ListUsersTests;

public class ListUsersTests(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationWithMembersTest(service, output, 3)
{
    [Fact]
    public async Task Can_list_organisation_users()
    {
        // arrange
        var ownerUserId = AdminUserContext.UserId;

        // act

        // assert
        var users = await Query(new ListUsers.Query(), AdminUserContext);
        users.Should().HaveCount(4);

        var owner = users.Single(x => x.Id == ownerUserId);
        owner.Role.Should().Be(UserRole.OwnerRoleName);

        var members = users.Where(x => x.Id != ownerUserId);
        foreach (var member in members)
        {
            MemberUserContexts.Select(x => x.UserId).Should().Contain(member.Id);
            member.Role.Should().Be(UserRole.MemberRoleName);
        }
    }
}