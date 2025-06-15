using Core.Application.Organisations.Exceptions;

namespace Core.IntegrationTests.Application.Organisations;

public class GetOrganisationTests(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Get_organisation_returns_organisation()
    {
        // arrange
        var command = new GetOrganisation.Query();

        // act
        var result = await QueryWithAdminContext(command);

        // assert
        result.Id.Should().Be(AdminUserContext.OrganisationId);
    }

    [Fact]
    public async Task Get_organisation_that_does_not_exists_throws_not_found_exception()
    {
        // arrange
        var command = new GetOrganisation.Query();
        var overrideUserContext =
            new UserContext(Guid.NewGuid(), Guid.NewGuid(), Fake.Internet.Email(), Fake.Internet.Password());

        // act
        Func<Task> act = async () => await Query(command, overrideUserContext);

        // assert
        await act.Should().ThrowAsync<OrganisationNotFoundException>();
    }
}