namespace Core.IntegrationTests.UseCases.Users;

public class UpdateNameTests(ServiceFixture service, ITestOutputHelper output) : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Can_update_user_name()
    {
        // arrange
        var userId = AdminUserContext.UserId;
        var command = new UpdateName.Command(userId, Fake.Name.FirstName(), Fake.Name.LastName());

        // act
        await ExecuteInAdminContext(command);

        // assert
        var result = await QueryWithAdminContext(new GetUser.Query(userId));
        result.FirstName.Should().Be(command.FirstName);
        result.LastName.Should().Be(command.LastName);
    }

    [Fact]
    public async Task User_must_exist()
    {
        // arrange
        var command = new UpdateName.Command(Guid.NewGuid(), Fake.Name.FirstName(), Fake.Name.LastName());

        // act
        var act = async () => await ExecuteInAdminContext(command);

        // assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}