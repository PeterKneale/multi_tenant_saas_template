namespace Core.IntegrationTests.UseCases.Users;

public class UpdateRoleTests(ServiceFixture service, ITestOutputHelper output) : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Can_demote_from_owner_to_member()
    {
        // arrange
        var command = new UpdateRole.Command(AdminUserContext.UserId, UserRole.MemberRoleName);

        // act
        await ExecuteInAdminContext(command);

        // assert
        var result = await QueryWithAdminContext(new GetUser.Query(AdminUserContext.UserId));
        result.Role.Should().Be(UserRole.MemberRoleName);
    }
}