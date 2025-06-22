using Core.Application.Organisations.Exceptions;

namespace Core.IntegrationTests.UseCases.Organisations;

public class UpdateNameTests(ServiceFixture service, ITestOutputHelper output) : SingleOrganisationTest(service, output)
{
    [Fact]
    public async Task Organisation_name_can_be_changed()
    {
        // arrange
        var title = Fake.Company.CompanyName();

        // act
        await ExecuteInAdminContext(new UpdateOrganisationName.Command(title));

        // assert
        var result = await QueryWithAdminContext(new GetOrganisation.Query());
        result.Title.Should().Be(title);
    }

    [Fact]
    public async Task Organisation_description_can_be_changed()
    {
        // arrange
        var title = Fake.Company.CompanyName();
        var description = Fake.Lorem.Sentence();

        // act
        await ExecuteInAdminContext(new UpdateOrganisationName.Command(title, description));

        // assert
        var result = await QueryWithAdminContext(new GetOrganisation.Query());
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
    }

    [Fact]
    public async Task Attempting_to_change_name_of_a_non_existent_organisation_fails()
    {
        // arrange
        var command = FakerExtensions.UpdateOrganisationName();

        // act
        var act = async () => await Command(command, NonExistentUserContext);

        // assert
        await act.Should().ThrowAsync<OrganisationNotFoundException>();
    }
}