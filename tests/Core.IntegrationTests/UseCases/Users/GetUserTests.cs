namespace Core.IntegrationTests.UseCases.Users;

public class GetUserTests(ServiceFixture service, ITestOutputHelper output) : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Get_user_that_does_not_exists_throws_not_found_exception()
    {
        // arrange
        var command = new GetUser.Query(Guid.NewGuid());

        // act
        Func<Task> act = async () => await QueryWithAdminContext(command);

        // assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}