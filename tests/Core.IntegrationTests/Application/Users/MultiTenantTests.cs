namespace Core.IntegrationTests.Application.Users;

public class MultiTenantTests(ServiceFixture service, ITestOutputHelper output)
    : MultipleOrganisationTest(service, output)
{
    [Fact]
    public async Task List_users_is_constrained_to_the_current_organisation()
    {
        // arrange

        // act
        var users1 = await Query(new ListUsers.Query(), Org1Admin);
        var users2 = await Query(new ListUsers.Query(), Org2Admin);

        // assert
        users1.Should().ContainSingle(x => x.Id == Org1Admin.UserId);
        users2.Should().ContainSingle(x => x.Id == Org2Admin.UserId);
        users1.Should().NotContain(x => x.Id == Org2Admin.UserId);
        users2.Should().NotContain(x => x.Id == Org1Admin.UserId);
    }

    [Fact]
    public async Task Get_user_is_constrained_to_the_current_organisation()
    {
        // arrange
        var query = new GetUser.Query(Org1Admin.UserId);

        // act
        Func<Task> act = async () => await Query(query, Org2Admin);

        // assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}